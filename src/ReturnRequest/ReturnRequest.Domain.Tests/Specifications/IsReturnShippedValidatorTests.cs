using ReturnRequest.Domain.Specifications;
using Xunit;

namespace ReturnRequest.Domain.Tests.Specifications;

public sealed class IsReturnShippedValidatorTests
{
    private readonly IsReturnShippedValidator _validator = new();

    [Fact]
    public void IsValid_WhenStatusIsReturnShipped_ReturnsTrue()
    {
        var model = TestAnemicModelBuilder.Build(status: "ReturnShipped");
        Assert.True(_validator.IsValid(model));
    }

    [Theory]
    [InlineData("Requested")]
    [InlineData("Approved")]
    [InlineData("Received")]
    [InlineData("Completed")]
    public void IsValid_WhenStatusIsNotReturnShipped_ReturnsFalse(string status)
    {
        var model = TestAnemicModelBuilder.Build(status: status);
        Assert.False(_validator.IsValid(model));
    }
}
