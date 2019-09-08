namespace WebApplication.ComponentTests.V1
{
    using FluentAssertions;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    internal sealed class LinkSteps
    {
        private readonly string linkEndpoint = "http://localhost/api/v1/link/";
        private readonly ServerFixture fixture;

        private object result;

        public LinkSteps(ServerFixture fixture)
        {
            this.fixture = fixture;
        }

        public LinkSteps GivenIHaveLinkInRepository(string shortUrl, Uri longUrl)
        {
            this.fixture.MockRepository
                .Setup(x => x.GetAsync(shortUrl))
                .ReturnsAsync(longUrl);

            return this;
        }

        public LinkSteps GivenISetupAddToRepo(string shortUrl, Uri longUrl)
        {
            this.fixture.MockShortenUrlGenerator
                .Setup(x => x.GetRandomUrl(longUrl))
                .Returns(shortUrl);

            this.fixture.MockRepository
                .Setup(x => x.AddAsync(shortUrl, longUrl))
                .ReturnsAsync(true);

            return this;
        }

        public async Task WhenIGetLongUrlAsync(string shortUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"{linkEndpoint}{shortUrl}"));
            this.result = await this.fixture.Client.SendAsync(request).ConfigureAwait(false);
        }

        public async Task WhenIShortenUrlAsync(string longUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{linkEndpoint}?url={longUrl}"));
            this.result = await this.fixture.Client.SendAsync(request).ConfigureAwait(false);
        }

        public async Task ThenResultShouldBeAsync(string expected)
        {
            this.result.Should().NotBeNull();

            var httpResponse = (HttpResponseMessage)this.result;
            httpResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var actual = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            actual.Should().Be(expected);
        }

        public LinkSteps ThenStatusCodeShouldBe(HttpStatusCode expected)
        {
            this.result.Should().NotBeNull();

            var httpResponse = (HttpResponseMessage)this.result;
            httpResponse.StatusCode.Should().Be((int)expected);

            return this;
        }
    }
}