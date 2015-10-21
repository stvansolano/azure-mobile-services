namespace Backend
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Threading.Tasks;
    using System;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class CloudContext
    {
        private Lazy<CloudStorageAccount> _storageAccount;

        public CloudContext(string connectionString)
        {
            ConnectionString = connectionString;
            _storageAccount = new Lazy<CloudStorageAccount>(() => {
                return CloudStorageAccount.Parse(ConnectionString);
            });
        }

        public string ConnectionString { get; private set; }

        public async Task<CloudBlobContainer> GetContainer(string containerName)
        {
            var blobClient = _storageAccount.Value.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(containerName ?? string.Empty);

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync().ConfigureAwait(false);

            return container;
        }

        public async Task<CloudTable> Table(string partitionKey)
        {
            var tableClient = _storageAccount.Value.CreateCloudTableClient();
            var tableReference = tableClient.GetTableReference(partitionKey);

            await tableReference.CreateIfNotExistsAsync().ConfigureAwait(false);

            return tableReference;
        }
    }
}