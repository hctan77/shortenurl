namespace WebApplication.UnitTests.Core.Model
{
    using FluentAssertions;
    using WebApplication.Core.Model;
    using Xunit;

    public sealed class LinkEntityTests
    {
        [Fact]
        public void Constructor_ValidArgument_ExpectedProperties()
        {
            var entity = new LinkEntity("abc", "https://www.example.com");

            entity.PartitionKey.Should().Be("a");
            entity.RowKey.Should().Be("abc");
            entity.Url.Should().Be("https://www.example.com");
        }

        [Fact]
        public void Constructor_EmptyParam_ExpectedProperties()
        {
            var entity = new LinkEntity();

            entity.PartitionKey.Should().Be(null);
            entity.RowKey.Should().Be(null);
            entity.Url.Should().Be(null);
        }
    }
}