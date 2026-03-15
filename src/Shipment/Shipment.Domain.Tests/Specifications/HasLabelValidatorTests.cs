using Shipment.Domain.Specifications;
using Xunit;

namespace Shipment.Domain.Tests.Specifications;

public sealed class HasLabelValidatorTests
{
    private readonly HasLabelValidator _validator = new();

    [Fact]
    public void IsValid_WhenLabelExists_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(labelUrl: "https://labels.example.com/label.pdf");
        Assert.True(_validator.IsSatisfiedBy(model));
    }

    [Fact]
    public void IsValid_WhenLabelIsNull_ReturnsFalse()
    {
        var model = TestAnemicModelBuilder.Build(labelUrl: null);
        Assert.False(_validator.IsSatisfiedBy(model));
    }
}
