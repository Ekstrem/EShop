namespace Promotion.Domain.Specifications;

using Promotion.Domain.Abstraction;

internal sealed class IsNotExpiredValidator
{
    public bool IsSatisfiedBy(IPromotionAnemicModel model)
        => model.Root.EndDate > DateTime.UtcNow;

    public string Reason => "Promotion has expired.";
}
