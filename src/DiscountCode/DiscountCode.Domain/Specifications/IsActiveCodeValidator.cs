namespace DiscountCode.Domain.Specifications;

using DiscountCode.Domain.Abstraction;

internal sealed class IsActiveCodeValidator
{
    public bool IsSatisfiedBy(IDiscountCodeAnemicModel model)
        => model.Root.Status == "Active";

    public string Reason => "Discount code must be in Active status.";
}
