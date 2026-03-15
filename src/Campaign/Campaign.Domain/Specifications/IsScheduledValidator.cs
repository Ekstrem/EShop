namespace Campaign.Domain.Specifications;

using Campaign.Domain.Abstraction;

internal sealed class IsScheduledValidator
{
    public bool IsSatisfiedBy(ICampaignAnemicModel model)
        => model.Root.Status == "Scheduled";

    public string Reason => "Campaign must be in Scheduled status.";
}
