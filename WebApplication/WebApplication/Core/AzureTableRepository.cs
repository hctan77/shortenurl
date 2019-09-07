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

    /// <summary>
    /// A class that implement <see cref="IRepository"/> using azure table.
    /// </summary>
    public sealed class AzureTableRepository : IRepository
    {
        private readonly CloudTable table;

        /// <summary>
        /// Initialize a new instance of <see cref="AzureTableRepository"/>
        /// </summary>
        /// <param name="options">Settings for the azure table.</param>
        /// <param name="cloudTable">Optional <see cref="CloudTable"/></param>
        public AzureTableRepository(AzureTableStorageOptions options, CloudTable cloudTable = null)
        {
            EnsureArg.IsNotNull(options, nameof(options));

            if (cloudTable == null)
            {
                StorageCredentials credentials = new StorageCredentials(options.AccountName, options.AccountKey);
                CloudStorageAccount account = new CloudStorageAccount(credentials, useHttps: true);

                var tableClient = account.CreateCloudTableClient();

                table = tableClient.GetTableReference(options.TableName);
            }
            else
            {
                this.table = cloudTable;
            }
        }

        /// <inheritdoc />
        public async Task<bool> AddAsync(string shortenUrl, Uri url)
        {
            EnsureArg.IsNotNullOrWhiteSpace(shortenUrl, nameof(shortenUrl));
            EnsureArg.IsNotNull(url, nameof(url));

            var entity = new LinkEntity(shortenUrl, url.ToString());

            TableOperation insertOperation = TableOperation.Insert(entity);

            try
            {
                await table.ExecuteAsync(insertOperation).ConfigureAwait(false);
            }
            catch (StorageException exception)
            {
                if (exception.RequestInformation?.HttpStatusCode == 409) // entity already exists
                {
                    return false;
                }

                throw;
            }

            return true;
        }

        /// <inheritdoc />
        public async Task<Uri> GetAsync(string shortenUrl)
        {
            EnsureArg.IsNotNullOrWhiteSpace(shortenUrl, nameof(shortenUrl));

            TableOperation retrieveOperation = TableOperation.Retrieve<LinkEntity>(shortenUrl.Substring(0,1), shortenUrl);
            TableResult query = await table.ExecuteAsync(retrieveOperation).ConfigureAwait(false);

            if (query == null)
            {
                return null;
            }

            var linkEntity = query.Result as LinkEntity;
            var url = linkEntity?.Url;

            if (!string.IsNullOrWhiteSpace(url) && Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri result))
            {
                return result;
            }

            return null;
        }
    }
}