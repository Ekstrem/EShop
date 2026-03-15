using Shipment.Domain.Specifications;
using Xunit;

namespace Shipment.Domain.Tests.Specifications;

public sealed class HasItemsValidatorTests
{
    private readonly HasItemsValidator _validator = new();

    [Fact]
    public void IsValid_WhenItemsExist_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(itemCount: 2);
        Assert.True(_validator.IsSatisfiedBy(model));
    }

    [Fact]
    public void IsValid_WhenNoItems_ReturnsFalse()
    {
        var model = TestAnemicModelBuilder.Build(itemCount: 0);
        Assert.False(_validator.IsSatisfiedBy(model));
    }
}
