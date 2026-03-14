namespace Cart.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain.Abstraction;

public class HasShippingAddressValidator : IBusinessOperationValidator<ICart, ICartAnemicModel>
{
    public bool IsSatisfiedBy(ICartAnemicModel model)
        => model.ShippingAddress != null;

    public string ErrorMessage => "Shipping address must be set before placing an order.";
}
