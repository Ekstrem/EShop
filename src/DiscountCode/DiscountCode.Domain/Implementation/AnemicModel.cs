namespace DiscountCode.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using DiscountCode.Domain.Abstraction;

internal sealed class AnemicModel : IDiscountCodeAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public IDiscountCodeRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IRedemption> Redemptions { get; internal set; } = new List<IRedemption>();

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
