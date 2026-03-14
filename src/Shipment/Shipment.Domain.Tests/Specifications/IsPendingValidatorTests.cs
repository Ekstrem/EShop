using Shipment.Domain.Specifications;
using Xunit;

namespace Shipment.Domain.Tests.Specifications;

public sealed class IsPendingValidatorTests
{
    private readonly IsPendingValidator _validator = new();

    [Fact]
    public void IsValid_WhenStatusIsPending_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(status: "Pending");
        Assert.True(_validator.IsValid(model));
    }

    [Theory]
    [InlineData("Packed")]
    [InlineData("Shipped")]
    [InlineData("InTransit")]
    [InlineData("Delivered")]
    public void IsValid_WhenStatusIsNotPending_ReturnsFalse(string status)
    {
        var model = TestAnemicModelBuilder.Build(status: status);
        Assert.False(_validator.IsValid(model));
    }
}
