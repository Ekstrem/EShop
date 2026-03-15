namespace Cart.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Cart.Domain.Abstraction;

internal sealed class AnemicModel : ICartAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public ICartRoot Root { get; internal set; } = null!;
    public IReadOnlyList<ICartItem> Items { get; internal set; } = new List<ICartItem>();
    public IPromoCode? AppliedPromoCode { get; internal set; }
    public IShippingAddress? ShippingAddress { get; internal set; }

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
