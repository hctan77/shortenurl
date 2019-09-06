namespace WebApplication.UnitTests.Core
{
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using WebApplication.Core;
    using Xunit;

    internal sealed class ShortenUrlGeneratorSteps
    {
        private Exception exception;

        public ShortenUrlGeneratorSteps WhenIInstantiateTheGeneratorWithNullFunc()
        {
            this.exception = Record.Exception(() => new ShortenUrlGenerator(null));
            return this;
        }

        public ShortenUrlGeneratorSteps ThenShouldThrowArgumentNullException()
        {
            this.exception.Should().BeOfType(typeof(ArgumentNullException));
            return this;
        }
    }
}