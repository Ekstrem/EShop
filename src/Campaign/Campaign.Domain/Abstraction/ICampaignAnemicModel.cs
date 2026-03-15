namespace Campaign.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface ICampaignAnemicModel : IAnemicModel<ICampaign>
{
    ICampaignRoot Root { get; }
    int TotalRecipients { get; }
    int SentCount { get; }
    int FailedCount { get; }
}
