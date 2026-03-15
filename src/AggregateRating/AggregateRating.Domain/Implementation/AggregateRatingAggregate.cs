namespace AggregateRating.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using AggregateRating.Domain.Abstraction;
using AggregateRating.Domain.Specifications;
using EShop.Contracts;

internal sealed class AggregateRatingAggregate
{
    public IAggregateRatingAnemicModel Model { get; }

    private AggregateRatingAggregate(IAggregateRatingAnemicModel model) => Model = model;

    public static AggregateRatingAggregate CreateInstance(IAggregateRatingAnemicModel model) => new(model);

    private AggregateResult<IAggregateRating, IAggregateRatingAnemicModel> Success(IAggregateRatingAnemicModel newModel)
    {
        var data = BusinessOperationData<IAggregateRating, IAggregateRatingAnemicModel>
            .Commit<IAggregateRating, IAggregateRatingAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<IAggregateRating, IAggregateRatingAnemicModel>(data);
    }

    private AggregateResult<IAggregateRating, IAggregateRatingAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<IAggregateRating, IAggregateRatingAnemicModel>
            .Commit<IAggregateRating, IAggregateRatingAnemicModel>(Model, Model);
        return new AggregateResultException<IAggregateRating, IAggregateRatingAnemicModel>(
            data, new FailedSpecification<IAggregateRating, IAggregateRatingAnemicModel>(error));
    }

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

        return Success(anemic);
    }

    /// <summary>
    /// Recalculates the aggregate rating based on current review data.
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
            return Fail(averageValidator.Reason);

        // Validate distribution consistency
        var distributionValidator = new DistributionConsistencyValidator();
        if (!distributionValidator.IsSatisfiedBy(anemic))
            return Fail(distributionValidator.Reason);

        // Validate publication threshold
        var thresholdValidator = new PublicationThresholdValidator();
        if (!thresholdValidator.IsSatisfiedBy(anemic) && status == "Active")
            return Fail(thresholdValidator.Reason);

        return Success(anemic);
    }
}
