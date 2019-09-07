namespace WebApplication.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xunit;

    public sealed class HomeControllerTests
    {
        private readonly HomeControllerSteps steps = new HomeControllerSteps();

        [Fact]
        public void Error_Execute_ReturnsErrorViewModel()
        {
            this.steps.WhenIExecuteError()
                .ThenViewResultShouldContainsErrorViewModel();
        }
    }
}