namespace TestCore.Azure
{
    using System.Collections.Generic;

    using EnsureThat;

    using Microsoft.WindowsAzure.Storage.Table;

    public sealed class GuardTableClient
    {
        private readonly List<CloudTable> createdTables = new List<CloudTable>();

        public GuardTableClient(CloudTableClient tableClient)
        {
            this.TableClient = EnsureArg.IsNotNull(tableClient, nameof(tableClient));
        }

        public CloudTableClient TableClient { get; }

        public CloudTable CreateTableIfNotExists(string tableName)
        {
            var table = this.TableClient.GetTableReference(tableName);
            if (table.CreateIfNotExistsAsync().Result)
            {
                this.createdTables.Add(table);
            }

            return table;
        }

        public void DeleteCreatedTables()
        {
            createdTables.ForEach(t => t.DeleteIfExistsAsync().Wait());
            createdTables.Clear();
        }
    }
}
