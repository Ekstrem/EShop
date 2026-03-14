namespace Campaign.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Campaign.Domain.Abstraction;

internal sealed class HasRequiredFieldsValidator : IBusinessOperationValidator<ICampaignAnemicModel>
{
    public bool IsSatisfiedBy(ICampaignAnemicModel model)
        => !string.IsNullOrWhiteSpace(model.Root.TemplateId)
           && !string.IsNullOrWhiteSpace(model.Root.Subject)
           && !string.IsNullOrWhiteSpace(model.Root.SegmentId);

    public string ErrorMessage => "Campaign must have templateId, subject, and segmentId.";
}
