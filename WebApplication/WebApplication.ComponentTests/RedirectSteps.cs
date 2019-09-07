namespace WebApplication.ComponentTests
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    internal sealed class RedirectSteps
    {
        private readonly string redirectEndpoint = "http://localhost/";
        private readonly ServerFixture fixture;

        private object result;

        public RedirectSteps(ServerFixture fixture)
        {
            this.fixture = fixture;
        }

        public RedirectSteps GivenIHaveLinkInRepository(string shortUrl, Uri longUrl)
        {
            this.fixture.MockRepository
                .Setup(x => x.GetAsync(shortUrl))
                .ReturnsAsync(longUrl);

            return this;
        }

        public async Task WhenIRedirectAsync(string shortUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"{redirectEndpoint}{shortUrl}"));
            this.result = await this.fixture.Client.SendAsync(request).ConfigureAwait(false);
        }

        public RedirectSteps ThenResultShouldBeRedirect(Uri expected)
        {
            var httpResponse = (HttpResponseMessage)this.result;
            httpResponse.StatusCode.Should().Be((int)HttpStatusCode.Redirect);
            httpResponse.Headers.TryGetValues("Location", out IEnumerable<string> values).Should().BeTrue();
            values.Single().Should().Be(expected.ToString());

            return this;
        }
    }
}