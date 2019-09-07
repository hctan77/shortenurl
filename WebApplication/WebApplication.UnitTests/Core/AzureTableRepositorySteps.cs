namespace WebApplication.UnitTests.Core
{
    using FluentAssertions;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using TestCore.Azure;
    using WebApplication.Configuration;
    using WebApplication.Core;
    using WebApplication.Core.Model;

    internal sealed class AzureTableRepositorySteps
    {
        private const string TableName = "TestAzureTable";
        private readonly CloudTable cloudTable;
        private readonly AzureTableRepository azureTableRepository;

        private bool addResult;
        private Uri result;

        public AzureTableRepositorySteps(AzureStorageEmulator azureStorageEmulator)
        {
            this.cloudTable = azureStorageEmulator.TableClient.CreateTableIfNotExists(TableName);
            this.azureTableRepository = new AzureTableRepository(new AzureTableStorageOptions(), this.cloudTable);
        }

        public Task GivenIHaveInAzureTable(string shortenUrl, string longUrl)
        {
            var entity = new LinkEntity(shortenUrl, longUrl);

            TableOperation insertOperation = TableOperation.Insert(entity);

            return this.cloudTable.ExecuteAsync(insertOperation);
        }

        public async Task WhenIAddAsync(string shortenUrl, Uri longUrl)
        {
            this.addResult = await this.azureTableRepository.AddAsync(shortenUrl, longUrl).ConfigureAwait(false);
        }

        public async Task WhenIGetAsync(string shortenUrl)
        {
            this.result = await this.azureTableRepository.GetAsync(shortenUrl).ConfigureAwait(false);
        }

        public async Task ThenIShouldHaveRecordInAzureTableAsync(string shortenUrl, string longUrl)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<LinkEntity>(shortenUrl.Substring(0, 1), shortenUrl);
            var tableResult = await this.cloudTable.ExecuteAsync(retrieveOperation).ConfigureAwait(false);
            var entity = tableResult.Result as LinkEntity;

            entity.Should().NotBeNull();
            entity.Url.Should().Be(longUrl);
            entity.PartitionKey.Should().Be(shortenUrl.Substring(0, 1));
            entity.RowKey.Should().Be(shortenUrl);
        }

        public AzureTableRepositorySteps ThenAddShouldBeSucessful()
        {
            this.addResult.Should().BeTrue();
            return this;
        }

        public AzureTableRepositorySteps ThenAddShouldBeFail()
        {
            this.addResult.Should().BeFalse();
            return this;
        }

        public AzureTableRepositorySteps ThenLongUrlShouldBe(Uri expected)
        {
            this.result.Should().Be(expected);
            return this;
        }
    }
}