namespace Backend
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    public abstract class StorageEntity
    {
        protected Action<DynamicTableEntity, StorageEntity> Map { get; set; }

        protected StorageEntity()
        {
            Tuple = new DynamicTableEntity();
        }

        private DynamicTableEntity _tuple;
        public DynamicTableEntity Tuple
        {
            get { return _tuple; }
            private set { _tuple = value; }
        }

        protected void AllocateProperty(string property, string defaultValue)
        {
            //Tuple.Properties[property] = new EntityProperty(defaultValue);
        }

        protected void SetValue(string property, string value)
        {
            Tuple.Properties[property].StringValue = value;
        }

        protected void SetValue(string property, bool? value)
        {
            Tuple.Properties[property].BooleanValue = value;
        }

        protected void SetValue(string property, int? value)
        {
            Tuple.Properties[property].Int32Value = value;
        }

        protected void SetValue(string property, DateTime? value)
        {
            //Tuple.Properties[property].DateTime = value;
        }

        protected void AllocateProperty(string property, bool defaultValue)
        {
            Tuple.Properties[property] = new EntityProperty(defaultValue);
        }

        protected void AllocateProperty(string property, DateTime? defaultValue)
        {
            //Tuple.Properties[property] = new EntityProperty(defaultValue);
        }

        protected void AllocateProperty(string property, int? defaultValue)
        {
            Tuple.Properties[property] = new EntityProperty(defaultValue);
        }

        public virtual void LoadFrom(DynamicTableEntity fetch)
        {
            Tuple = fetch;

            if (Map == null)
            {
                return;
            }
            try
            {
                Map(fetch, this);
            }
            catch
            {
            }
        }

        protected string FromStoredPropertyString(DynamicTableEntity storage, string property)
        {
            return storage.Properties[property].StringValue;
        }

        protected bool? FromStoredPropertyBoolean(DynamicTableEntity storage, string property)
        {
            return storage.Properties[property].BooleanValue;
        }

        protected DateTime? FromStoredPropertyDateTime(DynamicTableEntity storage, string property)
        {
            return /*storage.Properties[property].DateTime*/ null;
        }

        protected int? FromStoredPropertyInt32(DynamicTableEntity storage, string property)
        {
            return /*storage.Properties[property].Int32Value*/ null;
        }
    }
}