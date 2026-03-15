namespace Campaign.Domain.Specifications;

using Campaign.Domain.Abstraction;

internal sealed class IsDraftValidator
{
    public bool IsSatisfiedBy(ICampaignAnemicModel model)
        => model.Root.Status == "Draft";

    public string Reason => "Campaign must be in Draft status.";
}
