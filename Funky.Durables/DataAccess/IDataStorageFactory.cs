using Microsoft.Azure.Cosmos.Table;

namespace Funky.Durables.DataAccess
{
    public interface IDataStorageFactory
    {
        CloudTable GetTable();
    }

    public class DataStorageFactory : IDataStorageFactory
    {
        private readonly CloudTable _table;

        public DataStorageFactory(DatabaseConfiguration configuration)
        {
            var storageAccount = CloudStorageAccount.Parse(configuration.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(configuration.TableName);
            _table.CreateIfNotExists();
        }

        public CloudTable GetTable()
        {
            return _table;
        }
    }
}