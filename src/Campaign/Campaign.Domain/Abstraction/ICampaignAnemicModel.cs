namespace Campaign.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface ICampaignAnemicModel : IAnemicModel<ICampaign>
{
    ICampaignRoot Root { get; }
    int TotalRecipients { get; }
    int SentCount { get; }
    int FailedCount { get; }
}
