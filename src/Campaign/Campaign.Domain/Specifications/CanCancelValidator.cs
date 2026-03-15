namespace Campaign.Domain.Specifications;

using Campaign.Domain.Abstraction;

internal sealed class CanCancelValidator
{
    public bool IsSatisfiedBy(ICampaignAnemicModel model)
        => model.Root.Status is "Draft" or "Scheduled";

    public string Reason => "Only Draft or Scheduled campaigns can be cancelled.";
}
