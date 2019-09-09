namespace WebApplication.ComponentTests
{
    using FluentAssertions;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    internal sealed class StatusSteps
    {
        private readonly string statusEndpoint = "http://localhost/api/status";
        private readonly ServerFixture fixture;

        private object result;

        public StatusSteps(ServerFixture fixture)
        {
            this.fixture = fixture;
        }

        public async Task WhenIGetStatusAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(statusEndpoint));
            this.result = await this.fixture.Client.SendAsync(request).ConfigureAwait(false);
        }

        public async Task ThenStatusShouldBeAsync(string expected)
        {
            var httpResponse = (HttpResponseMessage)this.result;
            httpResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            content.Should().Be(expected);
        }
    }
}