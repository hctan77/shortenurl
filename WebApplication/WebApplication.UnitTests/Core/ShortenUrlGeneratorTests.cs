namespace WebApplication.UnitTests.Core
{
    using System;
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

        [Fact]
        public void GetRandomUrl_ValidUrl_ExpectedShortUrl()
        {
            this.steps
                .GivenIHaveAGeneratorWithoutRandomizer()
                .WhenIGetRandomUrl(new Uri("http://www.example.com"))
                .ThenShortUrlShouldBe("8XdxEf");
        }

        [Fact]
        public void GetRandomUrl_NullUrl_ThrowsException()
        {
            this.steps
                .GivenIHaveAGenerator()
                .WhenIGetRandomUrl(null)
                .ThenShouldThrowArgumentNullException();
        }
    }
}