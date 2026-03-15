namespace Promotion.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Promotion.Domain.Abstraction;

internal sealed class AnemicModel : IPromotionAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public IPromotionRoot Root { get; internal set; } = null!;
    public bool AllowStacking { get; internal set; }

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
