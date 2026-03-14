namespace DiscountCode.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using DiscountCode.Domain.Abstraction;

internal sealed class NotExpiredValidator : IBusinessOperationValidator<IDiscountCodeAnemicModel>
{
    public bool IsSatisfiedBy(IDiscountCodeAnemicModel model)
        => model.Root.ExpiresAt is null || model.Root.ExpiresAt > DateTime.UtcNow;

    public string ErrorMessage => "Discount code has expired.";
}
