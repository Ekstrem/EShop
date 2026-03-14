namespace Promotion.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Promotion.Domain.Abstraction;

internal sealed class IsActiveValidator : IBusinessOperationValidator<IPromotionAnemicModel>
{
    public bool IsSatisfiedBy(IPromotionAnemicModel model)
        => model.Root.Status == "Active";

    public string ErrorMessage => "Promotion must be in Active status.";
}
