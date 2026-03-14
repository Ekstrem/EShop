using Hive.SeedWorks.TacticalPatterns;
using AggregateRating.Domain.Abstraction;

namespace AggregateRating.Domain.Specifications;

/// <summary>
/// Validates that the average rating is between 1.0 and 5.0.
/// </summary>
internal sealed class AverageRangeValidator : IBusinessOperationValidator<IAggregateRatingAnemicModel>
{
    public bool IsSatisfiedBy(IAggregateRatingAnemicModel model)
        => model.Root.AverageRating >= 1.0m && model.Root.AverageRating <= 5.0m;

    public string ErrorMessage => "Average rating must be between 1.0 and 5.0.";
}
