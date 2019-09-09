namespace WebApplication.UnitTests.Controllers
{
    using Xunit;

    public sealed class StatusControllerTests
    {
        private readonly StatusControllerSteps steps = new StatusControllerSteps();

        [Fact]
        public void Get_ReturnsOK()
        {
            this.steps
                .WhenIGetStatus()
                .ThenStatusShouldBe("OK");
        }
    }
}