using Shipment.Domain.Specifications;
using Xunit;

namespace Shipment.Domain.Tests.Specifications;

public sealed class IsPackedValidatorTests
{
    private readonly IsPackedValidator _validator = new();

    [Fact]
    public void IsValid_WhenStatusIsPacked_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(status: "Packed");
        Assert.True(_validator.IsSatisfiedBy(model));
    }

    [Theory]
    [InlineData("Pending")]
    [InlineData("Shipped")]
    [InlineData("InTransit")]
    [InlineData("Delivered")]
    public void IsValid_WhenStatusIsNotPacked_ReturnsFalse(string status)
    {
        var model = TestAnemicModelBuilder.Build(status: status);
        Assert.False(_validator.IsSatisfiedBy(model));
    }
}
