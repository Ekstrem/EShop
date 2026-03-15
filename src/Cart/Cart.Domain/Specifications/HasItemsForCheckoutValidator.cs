namespace Cart.Domain.Specifications;

using Cart.Domain.Abstraction;

public class HasItemsForCheckoutValidator
{
    public bool IsSatisfiedBy(ICartAnemicModel model)
        => model.Items.Count > 0;

    public string Reason => "Cart must have at least one item to place an order.";
}
