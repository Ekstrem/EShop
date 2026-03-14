namespace Campaign.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Campaign.Domain.Abstraction;

internal sealed class CanCancelValidator : IBusinessOperationValidator<ICampaignAnemicModel>
{
    public bool IsSatisfiedBy(ICampaignAnemicModel model)
        => model.Root.Status is "Draft" or "Scheduled";

    public string ErrorMessage => "Only Draft or Scheduled campaigns can be cancelled.";
}
