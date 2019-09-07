namespace WebApplication.UnitTests.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using WebApplication.Controllers;
    using WebApplication.Models;

    internal sealed class HomeControllerSteps
    {
        private readonly HomeController controller;

        private object result;

        public HomeControllerSteps()
        {
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            this.controller = new HomeController()
            {
                ControllerContext = controllerContext
            };
        }

        public HomeControllerSteps WhenIExecuteError()
        {
            this.result = this.controller.Error();
            return this;
        }

        public HomeControllerSteps ThenViewResultShouldContainsErrorViewModel()
        {
            var viewResult = this.result as ViewResult;
            viewResult.Should().NotBeNull();

            var errorViewModel = viewResult.Model as ErrorViewModel;
            errorViewModel.Should().NotBeNull();
            errorViewModel.RequestId.Should().NotBeNull();
            
            return this;
        }
    }
}