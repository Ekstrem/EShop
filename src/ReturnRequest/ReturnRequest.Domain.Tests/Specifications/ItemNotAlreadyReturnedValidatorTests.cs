using ReturnRequest.Domain.Specifications;
using Xunit;

namespace ReturnRequest.Domain.Tests.Specifications;

public sealed class ItemNotAlreadyReturnedValidatorTests
{
    [Fact]
    public void IsValid_WhenNoItemsPreviouslyReturned_ReturnsTrue()
    {
        var alreadyReturned = new List<Guid>();
        var validator = new ItemNotAlreadyReturnedValidator(alreadyReturned);
        var model = TestAnemicModelBuilder.Build(itemCount: 2);
        Assert.True(validator.IsValid(model));
    }

    [Fact]
    public void IsValid_WhenItemAlreadyReturned_ReturnsFalse()
    {
        var model = TestAnemicModelBuilder.Build(itemCount: 1);
        var alreadyReturned = model.Items.Select(i => i.VariantId).ToList();
        var validator = new ItemNotAlreadyReturnedValidator(alreadyReturned);
        Assert.False(validator.IsValid(model));
    }

    [Fact]
    public void IsValid_WhenDifferentItemsReturned_ReturnsTrue()
    {
        var alreadyReturned = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var validator = new ItemNotAlreadyReturnedValidator(alreadyReturned);
        var model = TestAnemicModelBuilder.Build(itemCount: 2);
        Assert.True(validator.IsValid(model));
    }
}
