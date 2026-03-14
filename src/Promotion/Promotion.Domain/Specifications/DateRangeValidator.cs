namespace Promotion.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Promotion.Domain.Abstraction;

internal sealed class DateRangeValidator : IBusinessOperationValidator<IPromotionAnemicModel>
{
    public bool IsSatisfiedBy(IPromotionAnemicModel model)
        => model.Root.StartDate < model.Root.EndDate;

    public string ErrorMessage => "Start date must be before end date.";
}
