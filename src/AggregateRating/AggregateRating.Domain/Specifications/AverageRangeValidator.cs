namespace AggregateRating.Domain.Specifications;

using AggregateRating.Domain.Abstraction;

internal sealed class AverageRangeValidator
{
    public bool IsSatisfiedBy(IAggregateRatingAnemicModel model)
        => model.Root.AverageRating >= 1.0m && model.Root.AverageRating <= 5.0m;

    public string Reason => "Average rating must be between 1.0 and 5.0.";
}
