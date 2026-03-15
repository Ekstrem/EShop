using Shipment.Domain.Specifications;
using Xunit;

namespace Shipment.Domain.Tests.Specifications;

public sealed class SequentialStatusValidatorTests
{
    [Fact]
    public void IsValid_WhenTransitionIsForward_ReturnsTrue()
    {
        var validator = new SequentialStatusValidator("Packed");
        var model = TestAnemicModelBuilder.Build(status: "Pending");
        Assert.True(validator.IsSatisfiedBy(model));
    }

    [Fact]
    public void IsValid_WhenTransitionIsBackward_ReturnsFalse()
    {
        var validator = new SequentialStatusValidator("Pending");
        var model = TestAnemicModelBuilder.Build(status: "Packed");
        Assert.False(validator.IsSatisfiedBy(model));
    }

    [Fact]
    public void IsValid_WhenTransitionIsSameStatus_ReturnsFalse()
    {
        var validator = new SequentialStatusValidator("Shipped");
        var model = TestAnemicModelBuilder.Build(status: "Shipped");
        Assert.False(validator.IsSatisfiedBy(model));
    }

    [Fact]
    public void IsValid_WhenSkippingStatuses_ReturnsTrue()
    {
        var validator = new SequentialStatusValidator("Delivered");
        var model = TestAnemicModelBuilder.Build(status: "Shipped");
        Assert.True(validator.IsSatisfiedBy(model));
    }
}
