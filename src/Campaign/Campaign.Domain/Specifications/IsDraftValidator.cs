namespace Campaign.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Campaign.Domain.Abstraction;

internal sealed class IsDraftValidator : IBusinessOperationValidator<ICampaignAnemicModel>
{
    public bool IsSatisfiedBy(ICampaignAnemicModel model)
        => model.Root.Status == "Draft";

    public string ErrorMessage => "Campaign must be in Draft status.";
}
