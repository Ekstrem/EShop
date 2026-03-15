namespace Promotion.Domain.Specifications;

using Promotion.Domain.Abstraction;

internal sealed class IsDraftValidator
{
    public bool IsSatisfiedBy(IPromotionAnemicModel model)
        => model.Root.Status == "Draft";

    public string Reason => "Promotion must be in Draft status.";
}
