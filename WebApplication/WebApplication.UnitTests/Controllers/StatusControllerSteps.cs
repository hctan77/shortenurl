namespace WebApplication.UnitTests.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using WebApplication.Controllers;

    internal sealed class StatusControllerSteps
    {
        private readonly StatusController controller;
        private object result;

        public StatusControllerSteps()
        {
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            this.controller = new StatusController()
            {
                ControllerContext = controllerContext
            };
        }

        public StatusControllerSteps WhenIGetStatus()
        {
            this.result = this.controller.Get();
            return this;
        }

        public StatusControllerSteps ThenStatusShouldBe(string expected)
        {
            var url = this.result as ActionResult<string>;
            url.Value.Should().Be(expected);
            return this;
        }
    }
}