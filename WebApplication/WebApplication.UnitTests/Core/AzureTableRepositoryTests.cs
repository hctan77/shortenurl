namespace WebApplication.UnitTests.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using TestCore.Azure;
    using Xunit;

    public sealed class AzureTableRepositoryTests : IClassFixture<AzureStorageEmulator>
    {
        private readonly AzureTableRepositorySteps steps;

        public AzureTableRepositoryTests(AzureStorageEmulator azureStorageEmulator)
        {
            this.steps = new AzureTableRepositorySteps(azureStorageEmulator);
        }

        [Fact]
        public async Task AddAsync_NewEntity_Success()
        {
            await this.steps
                .WhenIAddAsync("Azzzzz6", new Uri("https://www.example.com/def")).ConfigureAwait(false);

            await this.steps
                .ThenAddShouldBeSucessful()
                .ThenIShouldHaveRecordInAzureTableAsync("Azzzzz6", "https://www.example.com/def").ConfigureAwait(false);
        }

        [Fact]
        public async Task AddAsync_EntityAlreadyExists_Fails()
        {
            await this.steps
                .GivenIHaveInAzureTable("b12345", "https://www.example.com").ConfigureAwait(false);

            await this.steps
                .WhenIAddAsync("b12345", new Uri("https://www.example.com")).ConfigureAwait(false);

            this.steps
                .ThenAddShouldBeFail();
        }

        [Fact]
        public async Task GetAsync_ExistingEntity_Success()
        {
            await this.steps
                .GivenIHaveInAzureTable("c12345", "https://www.example.com").ConfigureAwait(false);

            await this.steps
                .WhenIGetAsync("c12345").ConfigureAwait(false);

            this.steps.ThenLongUrlShouldBe(new Uri("https://www.example.com"));
        }

        [Fact]
        public async Task GetAsync_NotExists_Null()
        {
            await this.steps
                .WhenIGetAsync("d12345").ConfigureAwait(false);

            this.steps.ThenLongUrlShouldBe(null);
        }
    }
}