namespace WebApplication.UnitTests.Core
{
    using FluentAssertions;
    using System;
    using WebApplication.Core;
    using Xunit;

    internal sealed class ShortenUrlGeneratorSteps
    {
        private Exception exception;
        private ShortenUrlGenerator generator;
        private string result;

        public ShortenUrlGeneratorSteps GivenIHaveAGenerator()
        {
            this.generator = new ShortenUrlGenerator();
            return this;
        }

        public ShortenUrlGeneratorSteps GivenIHaveAGeneratorWithoutRandomizer()
        {
            this.generator = new ShortenUrlGenerator(() => string.Empty);
            return this;
        }

        public ShortenUrlGeneratorSteps WhenIInstantiateTheGeneratorWithNullFunc()
        {
            this.exception = Record.Exception(() => new ShortenUrlGenerator(null));
            return this;
        }

        public ShortenUrlGeneratorSteps WhenIGetRandomUrl(Uri url)
        {
            this.exception = Record.Exception(() => { this.result = this.generator.GetRandomUrl(url); });
            return this;
        }

        public ShortenUrlGeneratorSteps ThenShouldThrowArgumentNullException()
        {
            this.exception.Should().BeOfType(typeof(ArgumentNullException));
            return this;
        }

        public ShortenUrlGeneratorSteps ThenShortUrlShouldBe(string expected)
        {
            this.result.Should().Be(expected);
            return this;
        }
    }
}