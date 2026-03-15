namespace DiscountCode.Domain.Specifications;

using DiscountCode.Domain.Abstraction;

internal sealed class NotExpiredValidator
{
    public bool IsSatisfiedBy(IDiscountCodeAnemicModel model)
        => model.Root.ExpiresAt is null || model.Root.ExpiresAt > DateTime.UtcNow;

    public string Reason => "Discount code has expired.";
}
