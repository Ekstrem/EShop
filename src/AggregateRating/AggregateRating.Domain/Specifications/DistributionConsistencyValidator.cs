namespace AggregateRating.Domain.Specifications;

using AggregateRating.Domain.Abstraction;

internal sealed class DistributionConsistencyValidator
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

    public string Reason => "Rating distribution sum must equal total reviews.";
}
