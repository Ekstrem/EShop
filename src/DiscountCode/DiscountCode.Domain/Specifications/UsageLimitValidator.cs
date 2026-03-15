namespace DiscountCode.Domain.Specifications;

using DiscountCode.Domain.Abstraction;

internal sealed class UsageLimitValidator
{
    public bool IsSatisfiedBy(IDiscountCodeAnemicModel model)
        => model.Root.UsageCount < model.Root.MaxUsage;

    public string Reason => "Discount code usage limit has been reached.";
}
