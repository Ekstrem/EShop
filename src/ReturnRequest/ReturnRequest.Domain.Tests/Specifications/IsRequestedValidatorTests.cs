using ReturnRequest.Domain.Specifications;
using Xunit;

namespace ReturnRequest.Domain.Tests.Specifications;

public sealed class IsRequestedValidatorTests
{
    private readonly IsRequestedValidator _validator = new();

    [Fact]
    public void IsValid_WhenStatusIsRequested_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(status: "Requested");
        Assert.True(_validator.IsSatisfiedBy(model));
    }

    [Theory]
    [InlineData("Approved")]
    [InlineData("Rejected")]
    [InlineData("ReturnShipped")]
    [InlineData("Received")]
    [InlineData("Completed")]
    [InlineData("RejectedAfterInspection")]
    public void IsValid_WhenStatusIsNotRequested_ReturnsFalse(string status)
    {
        var model = TestAnemicModelBuilder.Build(status: status);
        Assert.False(_validator.IsSatisfiedBy(model));
    }
}
