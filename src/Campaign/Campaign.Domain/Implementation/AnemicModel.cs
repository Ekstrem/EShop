namespace Campaign.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Campaign.Domain.Abstraction;

internal sealed class AnemicModel : ICampaignAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public ICampaignRoot Root { get; internal set; } = null!;
    public int TotalRecipients { get; internal set; }
    public int SentCount { get; internal set; }
    public int FailedCount { get; internal set; }

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
