using ReturnRequest.Domain.Specifications;
using Xunit;

namespace ReturnRequest.Domain.Tests.Specifications;

public sealed class WithinReturnPeriodValidatorTests
{
    [Fact]
    public void IsValid_WhenWithin14Days_ReturnsTrue()
    {
        var deliveredAt = DateTime.UtcNow.AddDays(-10);
        var validator = new WithinReturnPeriodValidator(deliveredAt);
        var model = TestAnemicModelBuilder.Build(requestedAt: DateTime.UtcNow);
        Assert.True(validator.IsSatisfiedBy(model));
    }

    [Fact]
    public void IsValid_WhenExactly14Days_ReturnsTrue()
    {
        var now = DateTime.UtcNow;
        var deliveredAt = now.AddDays(-14);
        var validator = new WithinReturnPeriodValidator(deliveredAt);
        var model = TestAnemicModelBuilder.Build(requestedAt: now);
        Assert.True(validator.IsSatisfiedBy(model));
    }

    [Fact]
    public void IsValid_WhenBeyond14Days_ReturnsFalse()
    {
        var deliveredAt = DateTime.UtcNow.AddDays(-20);
        var validator = new WithinReturnPeriodValidator(deliveredAt);
        var model = TestAnemicModelBuilder.Build(requestedAt: DateTime.UtcNow);
        Assert.False(validator.IsSatisfiedBy(model));
    }
}
