namespace Cart.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain.Abstraction;

public class MaxItemsValidator : IBusinessOperationValidator<ICart, ICartAnemicModel>
{
    private const int MaxItems = 50;

    public bool IsSatisfiedBy(ICartAnemicModel model)
        => model.Items.Count < MaxItems;

    public string ErrorMessage => $"Cart cannot contain more than {MaxItems} items.";
}
