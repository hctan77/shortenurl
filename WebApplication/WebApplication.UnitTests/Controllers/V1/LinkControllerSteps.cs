namespace WebApplication.UnitTests.Controllers.V1
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using WebApplication.Configuration;
    using WebApplication.Controllers.V1;
    using WebApplication.Core;

    internal sealed class LinkControllerSteps
    {
        private readonly LinkController controller;
        private readonly Mock<IRepository> mockRepository = new Mock<IRepository>();
        private readonly Mock<IShortenUrlGenerator> mockUrlGenerator = new Mock<IShortenUrlGenerator>();

        private object result;

        public LinkControllerSteps()
        {
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            this.controller = new LinkController(mockRepository.Object, mockUrlGenerator.Object, new ApiOptions { AddMaxRetry = 2 })
            {
                ControllerContext = controllerContext
            };
        }

        public LinkControllerSteps GivenIHaveShortUrl(string shortUrl, Uri longUrl)
        {
            mockRepository.Setup(x => x.GetAsync(shortUrl)).ReturnsAsync(longUrl);
            return this;
        }

        public LinkControllerSteps GivenISetupUrlGeneratorToReturn(string shortUrl)
        {
            mockUrlGenerator.Setup(x => x.GetRandomUrl(It.IsAny<Uri>())).Returns(shortUrl);
            return this;
        }

        public LinkControllerSteps GivenISetupRepositoryAdd(string shortUrl, Uri longUrl, bool added)
        {
            mockRepository.Setup(x => x.AddAsync(shortUrl, longUrl)).ReturnsAsync(added);
            return this;
        }

        public async Task WhenICallShortenUrlAsync(Uri url)
        {
            this.result = await this.controller.ShortenAsync(url).ConfigureAwait(false);
        }

        public async Task WhenICallGetLongUrlAsync(string shortUrl)
        {
            this.result = await this.controller.GetLongUrlAsync(shortUrl).ConfigureAwait(false);
        }

        public LinkControllerSteps ThenShortenUrlShouldBe(string expected)
        {
            var url = this.result as ActionResult<string>;
            url.Value.Should().Be(expected);

            return this;
        }

        public LinkControllerSteps ThenLongUrlShouldBe(Uri expected)
        {
            var url = this.result as ActionResult<Uri>;
            url.Value.Should().Be(expected);

            return this;
        }

        public LinkControllerSteps ThenStatusCodeShouldBe(HttpStatusCode expected)
        {
            this.controller.Response.StatusCode.Should().Be((int)expected);
            return this;
        }

        public LinkControllerSteps ThenAddToRepositoryShouldBeCalled(int times)
        {
            mockRepository.Verify(x => x.AddAsync(It.IsAny<string>(), It.IsAny<Uri>()), Times.Exactly(times));
            return this;
        }
    }
}