namespace WebApplication.ComponentTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    [Collection("Server collection")]
    public sealed class RedirectTests
    {
        private readonly RedirectSteps steps;

        public RedirectTests(ServerFixture fixture)
        {
            this.steps = new RedirectSteps(fixture);
        }

        [Fact]
        public async Task Redirect_ValidShortUrl_RedirectsToLongUrl()
        {
            await this.steps
                .GivenIHaveLinkInRepository("abc", new Uri("http://www.example.com/1"))
                .WhenIRedirectAsync("abc").ConfigureAwait(false);

            this.steps
                .ThenResultShouldBeRedirect(new Uri("http://www.example.com/1"));
        }
    }
}