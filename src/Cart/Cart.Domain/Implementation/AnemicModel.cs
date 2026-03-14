namespace Cart.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain.Abstraction;

internal sealed class AnemicModel : AnemicModel<ICart>, ICartAnemicModel
{
    public ICartRoot Root { get; internal set; } = null!;
    public IReadOnlyList<ICartItem> Items { get; internal set; } = new List<ICartItem>();
    public IPromoCode? AppliedPromoCode { get; internal set; }
    public IShippingAddress? ShippingAddress { get; internal set; }
}
