namespace WebApplication.UnitTests.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xunit;

    public class ShortenUrlGeneratorTests
    {
        private readonly ShortenUrlGeneratorSteps steps = new ShortenUrlGeneratorSteps();

        [Fact]
        public void Constructor_NullFunc_ThrowsException()
        {
            this.steps
                .WhenIInstantiateTheGeneratorWithNullFunc()
                .ThenShouldThrowArgumentNullException();
        }
    }
}