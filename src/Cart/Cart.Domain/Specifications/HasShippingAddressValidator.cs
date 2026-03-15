namespace Cart.Domain.Specifications;

using Cart.Domain.Abstraction;

public class HasShippingAddressValidator
{
    public bool IsSatisfiedBy(ICartAnemicModel model)
        => model.ShippingAddress != null;

    public string Reason => "Shipping address must be set before placing an order.";
}
