using Shipment.Domain.Specifications;
using Xunit;

namespace Shipment.Domain.Tests.Specifications;

public sealed class HasTrackingNumberValidatorTests
{
    private readonly HasTrackingNumberValidator _validator = new();

    [Fact]
    public void IsValid_WhenTrackingNumberExists_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(trackingNumber: "TRK-12345");
        Assert.True(_validator.IsSatisfiedBy(model));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void IsValid_WhenTrackingNumberMissing_ReturnsFalse(string? trackingNumber)
    {
        var model = TestAnemicModelBuilder.Build(trackingNumber: trackingNumber ?? string.Empty);
        Assert.False(_validator.IsSatisfiedBy(model));
    }
}
