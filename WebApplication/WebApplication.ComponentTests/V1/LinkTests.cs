namespace WebApplication.ComponentTests.V1
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    [Collection("Server collection")]
    public sealed class LinkTests
    {
        private readonly LinkSteps steps;

        public LinkTests(ServerFixture fixture)
        {
            this.steps = new LinkSteps(fixture);
        }

        [Fact]
        public async Task GetLongUrlAsync_ValidShortUrl_ReturnsExpectedLongUrl()
        {
            await this.steps
                .GivenIHaveLinkInRepository("abc", new Uri("http://www.example.com/1"))
                .WhenIGetLongUrlAsync("abc").ConfigureAwait(false);

            await this.steps
                .ThenResultShouldBeAsync("\"http://www.example.com/1\"").ConfigureAwait(false);
        }

        [Fact]
        public async Task ShortenAsync_ShortUrl_ReturnsLongUrl()
        {
            await this.steps
                .GivenISetupAddToRepo("abc", new Uri("http://www.example.com/21"))
                .WhenIShortenUrlAsync(new Uri("http://www.example.com/21"));

            await this.steps
                .ThenResultShouldBeAsync("abc").ConfigureAwait(false);
        }
    }
}