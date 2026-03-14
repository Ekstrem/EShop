using ReturnRequest.Domain.Specifications;
using Xunit;

namespace ReturnRequest.Domain.Tests.Specifications;

public sealed class IsReceivedValidatorTests
{
    private readonly IsReceivedValidator _validator = new();

    [Fact]
    public void IsValid_WhenStatusIsReceived_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(status: "Received");
        Assert.True(_validator.IsValid(model));
    }

    [Theory]
    [InlineData("Requested")]
    [InlineData("Approved")]
    [InlineData("ReturnShipped")]
    [InlineData("Completed")]
    public void IsValid_WhenStatusIsNotReceived_ReturnsFalse(string status)
    {
        var model = TestAnemicModelBuilder.Build(status: status);
        Assert.False(_validator.IsValid(model));
    }
}
