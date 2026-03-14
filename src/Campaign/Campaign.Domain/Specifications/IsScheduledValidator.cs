namespace Campaign.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Campaign.Domain.Abstraction;

internal sealed class IsScheduledValidator : IBusinessOperationValidator<ICampaignAnemicModel>
{
    public bool IsSatisfiedBy(ICampaignAnemicModel model)
        => model.Root.Status == "Scheduled";

    public string ErrorMessage => "Campaign must be in Scheduled status.";
}
