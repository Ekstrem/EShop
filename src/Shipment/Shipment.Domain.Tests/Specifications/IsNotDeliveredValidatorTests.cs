using Shipment.Domain.Specifications;
using Xunit;

namespace Shipment.Domain.Tests.Specifications;

public sealed class IsNotDeliveredValidatorTests
{
    private readonly IsNotDeliveredValidator _validator = new();

    [Theory]
    [InlineData("Pending")]
    [InlineData("Packed")]
    [InlineData("Shipped")]
    [InlineData("InTransit")]
    public void IsValid_WhenNotDelivered_ReturnsTrue(string status)
    {
        var model = TestAnemicModelBuilder.Build(status: status);
        Assert.True(_validator.IsValid(model));
    }

    [Fact]
    public void IsValid_WhenDelivered_ReturnsFalse()
    {
        var model = TestAnemicModelBuilder.Build(status: "Delivered");
        Assert.False(_validator.IsValid(model));
    }
}
