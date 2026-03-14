namespace Cart.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain.Abstraction;

public class HasItemsForCheckoutValidator : IBusinessOperationValidator<ICart, ICartAnemicModel>
{
    public bool IsSatisfiedBy(ICartAnemicModel model)
        => model.Items.Count > 0;

    public string ErrorMessage => "Cart must have at least one item to place an order.";
}
