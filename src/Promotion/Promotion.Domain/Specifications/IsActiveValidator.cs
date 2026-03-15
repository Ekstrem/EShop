namespace Promotion.Domain.Specifications;

using Promotion.Domain.Abstraction;

internal sealed class IsActiveValidator
{
    public bool IsSatisfiedBy(IPromotionAnemicModel model)
        => model.Root.Status == "Active";

    public string Reason => "Promotion must be in Active status.";
}
