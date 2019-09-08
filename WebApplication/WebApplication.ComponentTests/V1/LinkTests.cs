namespace WebApplication.ComponentTests.V1
{
    using System;
    using System.Collections.Generic;
    using System.Net;
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
        public async Task ShortenAsync_ValidUrl_ReturnsShortUrl()
        {
            await this.steps
                .GivenISetupAddToRepo("abc", new Uri("http://www.example.com/21"))
                .WhenIShortenUrlAsync("http://www.example.com/21").ConfigureAwait(false);

            await this.steps
                .ThenResultShouldBeAsync("abc").ConfigureAwait(false);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("www.example.com")]
        public async Task ShortenAsync_InvalidUrl_BadRequest(string url)
        {
            await this.steps
                .WhenIShortenUrlAsync(url).ConfigureAwait(false);

            this.steps
                .ThenStatusCodeShouldBe(HttpStatusCode.BadRequest);
        }
    }
}