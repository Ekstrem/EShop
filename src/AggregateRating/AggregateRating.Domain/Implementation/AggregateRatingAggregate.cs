using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using AggregateRating.Domain.Abstraction;
using AggregateRating.Domain.Specifications;

namespace AggregateRating.Domain.Implementation;

/// <summary>
/// AggregateRating aggregate containing all business operations.
/// Each method returns an AggregateResult for event-driven processing.
/// </summary>
public sealed class AggregateRatingAggregate : Aggregate<IAggregateRating, IAggregateRatingAnemicModel>
{
    private AggregateRatingAggregate(IAggregateRatingAnemicModel model) : base(model) { }

    public static AggregateRatingAggregate CreateInstance(IAggregateRatingAnemicModel model) => new(model);

    /// <summary>
    /// Initializes a new aggregate rating for a product (triggered by ProductPublished event).
    /// </summary>
    public AggregateResult<IAggregateRating, IAggregateRatingAnemicModel> InitializeRating(Guid productId)
    {
        var root = AggregateRatingRoot.CreateInstance(
            Guid.NewGuid(), productId, 0m, 0, 0, "Pending");
        var distribution = RatingDistribution.Empty();

        var anemic = new AggregateRatingAnemicModel
        {
            Root = root,
            Distribution = distribution,
            WeightedAverage = 0m
        };

        return AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>.Ok(anemic);
    }

    /// <summary>
    /// Recalculates the aggregate rating based on current review data.
    /// Updates average, distribution, weighted average, and status.
    /// Verified purchases carry double weight in the weighted average.
    /// </summary>
    public AggregateResult<IAggregateRating, IAggregateRatingAnemicModel> RecalculateRating(
        int oneStar,
        int twoStar,
        int threeStar,
        int fourStar,
        int fiveStar,
        int verifiedReviews,
        int totalVerifiedRatingSum,
        int totalUnverifiedRatingSum)
    {
        var distribution = RatingDistribution.CreateInstance(
            oneStar, twoStar, threeStar, fourStar, fiveStar);

        var totalReviews = oneStar + twoStar + threeStar + fourStar + fiveStar;

        decimal averageRating = 0m;
        if (totalReviews > 0)
        {
            var sum = (1 * oneStar) + (2 * twoStar) + (3 * threeStar)
                      + (4 * fourStar) + (5 * fiveStar);
            averageRating = Math.Round((decimal)sum / totalReviews, 1);
        }

        // Weighted average: verified purchases count double
        decimal weightedAverage = 0m;
        var unverifiedReviews = totalReviews - verifiedReviews;
        var totalWeight = (verifiedReviews * 2) + unverifiedReviews;
        if (totalWeight > 0)
        {
            var weightedSum = (totalVerifiedRatingSum * 2) + totalUnverifiedRatingSum;
            weightedAverage = Math.Round((decimal)weightedSum / totalWeight, 1);
        }

        // Status: Active if 3+ published reviews, otherwise Pending
        var status = totalReviews >= 3 ? "Active" : "Pending";

        var root = AggregateRatingRoot.CreateInstance(
            Model.Root.Id,
            Model.Root.ProductId,
            averageRating,
            totalReviews,
            verifiedReviews,
            status);

        var anemic = new AggregateRatingAnemicModel
        {
            Root = root,
            Distribution = distribution,
            WeightedAverage = weightedAverage
        };

        // Validate average range
        var averageValidator = new AverageRangeValidator();
        if (totalReviews > 0 && !averageValidator.IsSatisfiedBy(anemic))
            return AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>.Fail(averageValidator.ErrorMessage);

        // Validate distribution consistency
        var distributionValidator = new DistributionConsistencyValidator();
        if (!distributionValidator.IsSatisfiedBy(anemic))
            return AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>.Fail(distributionValidator.ErrorMessage);

        // Validate publication threshold
        var thresholdValidator = new PublicationThresholdValidator();
        if (!thresholdValidator.IsSatisfiedBy(anemic) && status == "Active")
            return AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>.Fail(thresholdValidator.ErrorMessage);

        return AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>.Ok(anemic);
    }
}
