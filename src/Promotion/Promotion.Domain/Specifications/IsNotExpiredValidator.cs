namespace Promotion.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Promotion.Domain.Abstraction;

internal sealed class IsNotExpiredValidator : IBusinessOperationValidator<IPromotionAnemicModel>
{
    public bool IsSatisfiedBy(IPromotionAnemicModel model)
        => model.Root.EndDate > DateTime.UtcNow;

    public string ErrorMessage => "Promotion has expired.";
}
