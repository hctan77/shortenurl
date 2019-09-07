namespace WebApplication.UnitTests.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using WebApplication.Controllers;
    using WebApplication.Core;

    internal sealed class RedirectControllerSteps
    {
        private readonly RedirectController controller;
        private readonly Mock<IRepository> mockRepository = new Mock<IRepository>();

        private object result;

        public RedirectControllerSteps()
        {
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            this.controller = new RedirectController(mockRepository.Object)
            {
                ControllerContext = controllerContext
            };
        }

        public RedirectControllerSteps GivenIHaveShortUrl(string shortUrl, Uri longUrl)
        {
            mockRepository.Setup(x => x.GetAsync(shortUrl)).ReturnsAsync(longUrl);
            return this;
        }

        public async Task WhenICallIndexAsync(string shortUrl)
        {
            this.result = await this.controller.IndexAsync(shortUrl).ConfigureAwait(false);
        }

        public RedirectControllerSteps ThenResultShouldBeNotFound()
        {
            var notFoundResult = this.result as NotFoundResult;
            notFoundResult.Should().NotBeNull();

            return this;
        }

        public RedirectControllerSteps ThenResultShouldBeRedirect(Uri expected)
        {
            var redirectResult = this.result as RedirectResult;
            redirectResult.Should().NotBeNull();
            redirectResult.Url.Should().Be(expected.ToString());

            return this;
        }
    }
}