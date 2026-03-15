namespace Campaign.Domain.Specifications;

using Campaign.Domain.Abstraction;

internal sealed class HasRequiredFieldsValidator
{
    public bool IsSatisfiedBy(ICampaignAnemicModel model)
        => !string.IsNullOrWhiteSpace(model.Root.TemplateId)
           && !string.IsNullOrWhiteSpace(model.Root.Subject)
           && !string.IsNullOrWhiteSpace(model.Root.SegmentId);

    public string Reason => "Campaign must have templateId, subject, and segmentId.";
}
