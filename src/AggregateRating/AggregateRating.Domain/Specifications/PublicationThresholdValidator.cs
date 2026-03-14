using Hive.SeedWorks.TacticalPatterns;
using AggregateRating.Domain.Abstraction;

namespace AggregateRating.Domain.Specifications;

/// <summary>
/// Validates that there are at least 3 published reviews for Active status.
/// </summary>
internal sealed class PublicationThresholdValidator : IBusinessOperationValidator<IAggregateRatingAnemicModel>
{
    public bool IsSatisfiedBy(IAggregateRatingAnemicModel model)
        => model.Root.TotalReviews >= 3;

    public string ErrorMessage => "At least 3 published reviews are required for Active status.";
}
