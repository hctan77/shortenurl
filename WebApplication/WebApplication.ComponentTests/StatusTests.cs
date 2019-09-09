namespace WebApplication.ComponentTests
{
    using System.Threading.Tasks;
    using Xunit;

    [Collection("Server collection")]
    public sealed class StatusTests
    {
        private readonly StatusSteps steps;

        public StatusTests(ServerFixture fixture)
        {
            this.steps = new StatusSteps(fixture);
        }

        [Fact]
        public async Task GetStatus_ReturnsOK()
        {
            await this.steps
                .WhenIGetStatusAsync().ConfigureAwait(false);

            await this.steps
                .ThenStatusShouldBeAsync("OK").ConfigureAwait(false);
        }
    }
}