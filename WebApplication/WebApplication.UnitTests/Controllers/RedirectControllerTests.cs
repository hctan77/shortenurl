namespace WebApplication.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class RedirectControllerTests
    {
        private readonly RedirectControllerSteps steps = new RedirectControllerSteps();

        [Fact]
        public async Task IndexAsync_NullShortUrl_NotFound()
        {
            await this.steps
                .WhenICallIndexAsync(null).ConfigureAwait(false);

            this.steps
                .ThenResultShouldBeNotFound();
        }

        [Fact]
        public async Task IndexAsync_NonExistsShortUrl_NotFound()
        {
            await this.steps
                .GivenIHaveShortUrl("abc", null)
                .WhenICallIndexAsync("abc").ConfigureAwait(false);

            this.steps
                .ThenResultShouldBeNotFound();
        }

        [Fact]
        public async Task IndexAsync_ExistingShortUrl_Redirect()
        {
            var longUrl = new Uri("http://www.example.com");

            await this.steps
                .GivenIHaveShortUrl("abc", longUrl)
                .WhenICallIndexAsync("abc").ConfigureAwait(false);

            this.steps
                .ThenResultShouldBeRedirect(longUrl);
        }
    }
}