using ReturnRequest.Domain.Specifications;
using Xunit;

namespace ReturnRequest.Domain.Tests.Specifications;

public sealed class HasReasonValidatorTests
{
    private readonly HasReasonValidator _validator = new();

    [Fact]
    public void IsValid_WhenReasonProvided_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(reason: "Product is defective");
        Assert.True(_validator.IsSatisfiedBy(model));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_WhenReasonMissing_ReturnsFalse(string reason)
    {
        var model = TestAnemicModelBuilder.Build(reason: reason);
        Assert.False(_validator.IsSatisfiedBy(model));
    }
}
