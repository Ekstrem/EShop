namespace DiscountCode.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using DiscountCode.Domain.Abstraction;

internal sealed class UsageLimitValidator : IBusinessOperationValidator<IDiscountCodeAnemicModel>
{
    public bool IsSatisfiedBy(IDiscountCodeAnemicModel model)
        => model.Root.UsageCount < model.Root.MaxUsage;

    public string ErrorMessage => "Discount code usage limit has been reached.";
}
