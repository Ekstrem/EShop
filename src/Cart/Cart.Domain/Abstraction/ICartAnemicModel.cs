namespace Cart.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface ICartAnemicModel : IAnemicModel<ICart>
{
    ICartRoot Root { get; }
    IReadOnlyList<ICartItem> Items { get; }
    IPromoCode? AppliedPromoCode { get; }
    IShippingAddress? ShippingAddress { get; }
}
