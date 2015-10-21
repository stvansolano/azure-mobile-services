namespace Backend
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class RepositoryBase<Entity>
        where Entity : StorageEntity, new()
    {
        private CloudContext Context;
        private ConcurrentQueue<Tuple<ITableEntity, TableOperation>> _operations;

        protected RepositoryBase(string partitionKey, CloudContext context)
        {
            PartitionKey = partitionKey;
            Context = context;

            _operations = new ConcurrentQueue<Tuple<ITableEntity, TableOperation>>();
        }

        public Entity Create()
        {
            var entity = new Entity();
            entity.Tuple.PartitionKey = PartitionKey;

            return entity;
        }

        protected void Insert<TEntity>(TEntity entity)
            where TEntity : ITableEntity
        {
            if (string.IsNullOrEmpty(entity.RowKey))
            {
                entity.RowKey = Guid.NewGuid().ToString();
            }
            if (string.IsNullOrEmpty(entity.PartitionKey))
            {
                entity.PartitionKey = PartitionKey;
            }
            var e = new Tuple<ITableEntity, TableOperation>(entity, TableOperation.Insert(entity));

            _operations.Enqueue(e);
        }

        public string PartitionKey { get; private set; }

        public async Task<IEnumerable<Entity>> GetAll()
        {
            var query = new TableQuery<DynamicTableEntity>()
                                                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey)/*,
                                                    TableOperators.And,
                                                    TableQuery.CombineFilters(
                                                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "row1"),
                                                        TableOperators.Or,
                                                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "row2"))*/);

            return await ExecuteQuery(query);
        }

        protected virtual async Task<IEnumerable<Entity>> Find(string property, string value)
        {
            var query = new TableQuery<DynamicTableEntity>()
                                               .Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey),
                                                    TableOperators.And,
                                                    TableQuery.GenerateFilterCondition(property, QueryComparisons.Equal, value))
                                               );

            return await ExecuteQuery(query);
        }

        private async Task<IEnumerable<Entity>> ExecuteQuery(TableQuery<DynamicTableEntity> query)
        {
            var token = new TableContinuationToken();

            var table = await Context.Table(PartitionKey).ConfigureAwait(false);
            var segment = await table.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(false);

            var results = (from fetch in segment.ToArray()
                           let result = new { Entity = new Entity(), Result = fetch }
                           select result).ToArray();

            foreach (var item in results)
            {
                item.Entity.LoadFrom(item.Result);
            }
            return results.Select(item => item.Entity).ToArray();
        }

        public async Task<Entity> Find(string rowKey)
        {
            var retrieveOperation = TableOperation.Retrieve(PartitionKey, rowKey);
            var table = await Context.Table(PartitionKey).ConfigureAwait(false);

            var retrievedResult = await table.ExecuteAsync(retrieveOperation).ConfigureAwait(false);
            var fetch = retrievedResult.Result as DynamicTableEntity;
            var entity = new Entity();

            if (fetch != null)
            {
                entity.LoadFrom(fetch);
            }

            return entity;
        }

        public void Commit()
        {
            var count = _operations.Count;
            var toExecute = new List<Tuple<ITableEntity, TableOperation>>();
            for (var index = 0; index < count; index++)
            {
                Tuple<ITableEntity, TableOperation> operation;
                _operations.TryDequeue(out operation);
                if (operation != null)
                    toExecute.Add(operation);
            }

            toExecute
               .GroupBy(tuple => tuple.Item1.PartitionKey)
               .ToList()
               .ForEach(g =>
               {
                   var opreations = g.ToList();

                   var batch = 0;
                   var operationBatch = GetOperations(opreations, batch);

                   while (operationBatch.Any())
                   {
                       var tableBatchOperation = MakeBatchOperation(operationBatch);

                       ExecuteBatchWithRetries(tableBatchOperation);

                       batch++;
                       operationBatch = GetOperations(opreations, batch);
                   }
               });
        }


        private async void ExecuteBatchWithRetries(TableBatchOperation tableBatchOperation)
        {
            var tableRequestOptions = MakeTableRequestOptions();

            var tableReference = await Context.Table(PartitionKey).ConfigureAwait(false);

            await tableReference.ExecuteBatchAsync(tableBatchOperation).ConfigureAwait(false);
        }

        private static TableRequestOptions MakeTableRequestOptions()
        {
            return new TableRequestOptions
            {
                RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(2),
                                                       100)
            };
        }

        private static TableBatchOperation MakeBatchOperation(List<Tuple<ITableEntity, TableOperation>> operationsToExecute)
        {
            var tableBatchOperation = new TableBatchOperation();
            operationsToExecute.ForEach(tuple => tableBatchOperation.Add(tuple.Item2));
            return tableBatchOperation;
        }


        private static List<Tuple<ITableEntity, TableOperation>> GetOperations( IEnumerable<Tuple<ITableEntity, TableOperation>> operations, int batch)
        {
            return operations
                .Skip(batch * BatchSize)
                .Take(BatchSize)
                .ToList();
        }

        private const int BatchSize = 100;
    }
}