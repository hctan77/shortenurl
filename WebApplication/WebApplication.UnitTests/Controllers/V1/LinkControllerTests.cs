namespace WebApplication.UnitTests.Controllers.V1
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class LinkControllerTests
    {
        private readonly LinkControllerSteps steps = new LinkControllerSteps();
        private readonly Uri sampleUrl = new Uri("http://www.example.com");

        [Fact]
        public async Task ShortenAsync_NullUrl_BadRequest()
        {
            await this.steps
                .WhenICallShortenUrlAsync(null).ConfigureAwait(false);

            this.steps
                .ThenStatusCodeShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ShortenAsync_FailedToAdd_InternalServerError()
        {
            await this.steps
                .GivenISetupRepositoryAdd("abc", this.sampleUrl, false)
                .GivenISetupUrlGeneratorToReturn("abc")
                .WhenICallShortenUrlAsync(this.sampleUrl).ConfigureAwait(false);

            this.steps
                .ThenAddToRepositoryShouldBeCalled(2)
                .ThenStatusCodeShouldBe(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task ShortenAsync_AddSuccessful_ReturnsShortUrl()
        {
            await this.steps
                .GivenISetupRepositoryAdd("abc", this.sampleUrl, true)
                .GivenISetupUrlGeneratorToReturn("abc")
                .WhenICallShortenUrlAsync(this.sampleUrl).ConfigureAwait(false);

            this.steps
                .ThenAddToRepositoryShouldBeCalled(1)
                .ThenStatusCodeShouldBe(HttpStatusCode.OK)
                .ThenShortenUrlShouldBe("abc");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetLongUrlAsync_InvalidShortUrl_BadRequest(string shortUrl)
        {
            await this.steps
                .WhenICallGetLongUrlAsync(shortUrl).ConfigureAwait(false);

            this.steps
                .ThenStatusCodeShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetLongUrlAsync_ValidShortUrl_ReturnsLongUrl()
        {
            var shortUrl = "abc";
            var longUrl = new Uri("http://www.example.com");

            await this.steps
                .GivenIHaveShortUrl(shortUrl, longUrl)
                .WhenICallGetLongUrlAsync(shortUrl).ConfigureAwait(false);

            this.steps
                .ThenStatusCodeShouldBe(HttpStatusCode.OK)
                .ThenLongUrlShouldBe(longUrl);
        }

        [Fact]
        public async Task GetLongUrlAsync_ShortUrlNotFound_NotFound()
        {
            var shortUrl = "abc";

            await this.steps
                .GivenIHaveShortUrl(shortUrl, null)
                .WhenICallGetLongUrlAsync(shortUrl).ConfigureAwait(false);

            this.steps
                .ThenStatusCodeShouldBe(HttpStatusCode.NotFound);
        }
    }
}