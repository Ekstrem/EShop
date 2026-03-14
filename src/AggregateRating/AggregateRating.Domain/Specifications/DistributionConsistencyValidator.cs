using Hive.SeedWorks.TacticalPatterns;
using AggregateRating.Domain.Abstraction;

namespace AggregateRating.Domain.Specifications;

/// <summary>
/// Validates that the sum of the rating distribution equals total reviews.
/// </summary>
internal sealed class DistributionConsistencyValidator : IBusinessOperationValidator<IAggregateRatingAnemicModel>
{
    public bool IsSatisfiedBy(IAggregateRatingAnemicModel model)
    {
        var distributionSum = model.Distribution.OneStar
            + model.Distribution.TwoStar
            + model.Distribution.ThreeStar
            + model.Distribution.FourStar
            + model.Distribution.FiveStar;

        return distributionSum == model.Root.TotalReviews;
    }

    public string ErrorMessage => "Rating distribution sum must equal total reviews.";
}
