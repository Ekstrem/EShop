namespace Promotion.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Promotion.Domain.Abstraction;

internal sealed class IsDraftValidator : IBusinessOperationValidator<IPromotionAnemicModel>
{
    public bool IsSatisfiedBy(IPromotionAnemicModel model)
        => model.Root.Status == "Draft";

    public string ErrorMessage => "Promotion must be in Draft status.";
}
