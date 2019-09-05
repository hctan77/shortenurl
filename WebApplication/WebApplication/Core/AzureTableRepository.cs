namespace WebApplication.Core
{
    using EnsureThat;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Threading.Tasks;
    using WebApplication.Configuration;
    using WebApplication.Core.Model;

    public sealed class AzureTableRepository : IRepository
    {
        private readonly CloudTable table;

        public AzureTableRepository(AzureTableStorageOptions options)
        {
            EnsureArg.IsNotNull(options, nameof(options));

            StorageCredentials credentials = new StorageCredentials(options.AccountName, options.AccountKey);
            CloudStorageAccount account = new CloudStorageAccount(credentials, useHttps: true);

            var tableClient = account.CreateCloudTableClient();

            table = tableClient.GetTableReference(options.TableName);
        }

        /// <inheritdoc />
        public async Task<bool> AddAsync(string shortenUrl, Uri url)
        {
            var entity = new LinkEntity(shortenUrl, url.ToString());

            TableOperation insertOperation = TableOperation.Insert(entity);

            var tableResult = await table.ExecuteAsync(insertOperation).ConfigureAwait(false);

            if (tableResult.HttpStatusCode == 409) // entity already exists
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public Uri Get(string shortenUrl)
        {
            throw new NotImplementedException();
        }
    }
}