using ReturnRequest.Domain.Specifications;
using Xunit;

namespace ReturnRequest.Domain.Tests.Specifications;

public sealed class RefundAmountValidatorTests
{
    private readonly RefundAmountValidator _validator = new();

    [Fact]
    public void IsValid_WhenRefundEqualsItemsCost_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(
            itemCount: 2,
            unitPrice: 25.00m,
            refundAmount: 50.00m);
        Assert.True(_validator.IsSatisfiedBy(model));
    }

    [Fact]
    public void IsValid_WhenRefundLessThanItemsCost_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(
            itemCount: 2,
            unitPrice: 25.00m,
            refundAmount: 30.00m);
        Assert.True(_validator.IsSatisfiedBy(model));
    }

    [Fact]
    public void IsValid_WhenRefundExceedsItemsCost_ReturnsFalse()
    {
        var model = TestAnemicModelBuilder.Build(
            itemCount: 1,
            unitPrice: 25.00m,
            refundAmount: 50.00m);
        Assert.False(_validator.IsSatisfiedBy(model));
    }

    [Fact]
    public void IsValid_WhenRefundIsZero_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(
            itemCount: 1,
            unitPrice: 25.00m,
            refundAmount: 0m);
        Assert.True(_validator.IsSatisfiedBy(model));
    }

    [Fact]
    public void IsValid_WhenRefundIsNegative_ReturnsFalse()
    {
        var model = TestAnemicModelBuilder.Build(
            itemCount: 1,
            unitPrice: 25.00m,
            refundAmount: -5.00m);
        Assert.False(_validator.IsSatisfiedBy(model));
    }
}
