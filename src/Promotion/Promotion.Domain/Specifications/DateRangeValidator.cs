namespace Promotion.Domain.Specifications;

using Promotion.Domain.Abstraction;

internal sealed class DateRangeValidator
{
    public bool IsSatisfiedBy(IPromotionAnemicModel model)
        => model.Root.StartDate < model.Root.EndDate;

    public string Reason => "Start date must be before end date.";
}
