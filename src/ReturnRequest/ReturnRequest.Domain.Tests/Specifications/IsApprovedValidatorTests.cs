using ReturnRequest.Domain.Specifications;
using Xunit;

namespace ReturnRequest.Domain.Tests.Specifications;

public sealed class IsApprovedValidatorTests
{
    private readonly IsApprovedValidator _validator = new();

    [Fact]
    public void IsValid_WhenStatusIsApproved_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(status: "Approved");
        Assert.True(_validator.IsValid(model));
    }

    [Theory]
    [InlineData("Requested")]
    [InlineData("Rejected")]
    [InlineData("ReturnShipped")]
    [InlineData("Received")]
    [InlineData("Completed")]
    public void IsValid_WhenStatusIsNotApproved_ReturnsFalse(string status)
    {
        var model = TestAnemicModelBuilder.Build(status: status);
        Assert.False(_validator.IsValid(model));
    }
}
