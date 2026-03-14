namespace Promotion.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Promotion.Domain.Abstraction;

internal sealed class DiscountValueValidator : IBusinessOperationValidator<IPromotionAnemicModel>
{
    public bool IsSatisfiedBy(IPromotionAnemicModel model)
    {
        if (model.Root.DiscountValue <= 0)
            return false;

        if (model.Root.DiscountType == "Percentage" && model.Root.DiscountValue > 100)
            return false;

        return true;
    }

    public string ErrorMessage => "Discount value must be greater than 0 and percentage must not exceed 100.";
}
