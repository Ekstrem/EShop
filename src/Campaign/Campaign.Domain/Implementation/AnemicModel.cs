namespace Campaign.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Campaign.Domain.Abstraction;

internal sealed class AnemicModel : AnemicModel<ICampaign>, ICampaignAnemicModel
{
    public ICampaignRoot Root { get; internal set; } = null!;
    public int TotalRecipients { get; internal set; }
    public int SentCount { get; internal set; }
    public int FailedCount { get; internal set; }
}
