namespace DiscountCode.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using DiscountCode.Domain.Abstraction;

internal sealed class IsActiveCodeValidator : IBusinessOperationValidator<IDiscountCodeAnemicModel>
{
    public bool IsSatisfiedBy(IDiscountCodeAnemicModel model)
        => model.Root.Status == "Active";

    public string ErrorMessage => "Discount code must be in Active status.";
}
