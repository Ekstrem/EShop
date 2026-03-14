using AggregateRating.Domain.Implementation;
using Xunit;

namespace AggregateRating.Domain.Tests;

public sealed class AggregateRatingAggregateTests
{
    private static AggregateRatingAggregate CreateEmptyAggregate()
        => AggregateRatingAggregate.CreateInstance(new AggregateRatingAnemicModel());

    private static AggregateRatingAggregate CreateExistingAggregate(Guid? productId = null)
    {
        var pid = productId ?? Guid.NewGuid();
        var root = AggregateRatingRoot.CreateInstance(
            Guid.NewGuid(), pid, 0m, 0, 0, "Pending");
        var model = new AggregateRatingAnemicModel
        {
            Root = root,
            Distribution = RatingDistribution.Empty(),
            WeightedAverage = 0m
        };
        return AggregateRatingAggregate.CreateInstance(model);
    }

    [Fact]
    public void InitializeRating_Succeeds()
    {
        var aggregate = CreateEmptyAggregate();
        var productId = Guid.NewGuid();

        var result = aggregate.InitializeRating(productId);

        Assert.True(result.IsSuccess);
        Assert.Equal(productId, result.Model.Root.ProductId);
        Assert.Equal("Pending", result.Model.Root.Status);
        Assert.Equal(0, result.Model.Root.TotalReviews);
    }

    [Fact]
    public void RecalculateRating_WithThreeOrMoreReviews_StatusIsActive()
    {
        var aggregate = CreateExistingAggregate();

        var result = aggregate.RecalculateRating(
            oneStar: 0, twoStar: 0, threeStar: 1, fourStar: 1, fiveStar: 1,
            verifiedReviews: 2,
            totalVerifiedRatingSum: 9,
            totalUnverifiedRatingSum: 3);

        Assert.True(result.IsSuccess);
        Assert.Equal("Active", result.Model.Root.Status);
        Assert.Equal(3, result.Model.Root.TotalReviews);
    }

    [Fact]
    public void RecalculateRating_WithFewerThanThreeReviews_StatusIsPending()
    {
        var aggregate = CreateExistingAggregate();

        var result = aggregate.RecalculateRating(
            oneStar: 0, twoStar: 0, threeStar: 0, fourStar: 1, fiveStar: 1,
            verifiedReviews: 1,
            totalVerifiedRatingSum: 5,
            totalUnverifiedRatingSum: 4);

        Assert.True(result.IsSuccess);
        Assert.Equal("Pending", result.Model.Root.Status);
        Assert.Equal(2, result.Model.Root.TotalReviews);
    }

    [Fact]
    public void RecalculateRating_CalculatesCorrectAverage()
    {
        var aggregate = CreateExistingAggregate();

        // 1*1 + 2*0 + 3*0 + 4*2 + 5*2 = 1 + 0 + 0 + 8 + 10 = 19 / 5 = 3.8
        var result = aggregate.RecalculateRating(
            oneStar: 1, twoStar: 0, threeStar: 0, fourStar: 2, fiveStar: 2,
            verifiedReviews: 3,
            totalVerifiedRatingSum: 14,
            totalUnverifiedRatingSum: 5);

        Assert.True(result.IsSuccess);
        Assert.Equal(3.8m, result.Model.Root.AverageRating);
    }

    [Fact]
    public void RecalculateRating_CalculatesWeightedAverage_WithVerifiedDoubleWeight()
    {
        var aggregate = CreateExistingAggregate();

        // 3 verified (sum=12), 1 unverified (sum=2)
        // weighted = (12*2 + 2) / (3*2 + 1) = 26 / 7 = 3.7 (rounded)
        var result = aggregate.RecalculateRating(
            oneStar: 0, twoStar: 1, threeStar: 0, fourStar: 2, fiveStar: 1,
            verifiedReviews: 3,
            totalVerifiedRatingSum: 12,
            totalUnverifiedRatingSum: 2);

        Assert.True(result.IsSuccess);
        Assert.Equal(3.7m, result.Model.WeightedAverage);
    }

    [Fact]
    public void RecalculateRating_DistributionConsistency_Holds()
    {
        var aggregate = CreateExistingAggregate();

        var result = aggregate.RecalculateRating(
            oneStar: 2, twoStar: 3, threeStar: 5, fourStar: 4, fiveStar: 6,
            verifiedReviews: 10,
            totalVerifiedRatingSum: 35,
            totalUnverifiedRatingSum: 30);

        Assert.True(result.IsSuccess);
        Assert.Equal(20, result.Model.Root.TotalReviews);

        var dist = result.Model.Distribution;
        var sum = dist.OneStar + dist.TwoStar + dist.ThreeStar + dist.FourStar + dist.FiveStar;
        Assert.Equal(result.Model.Root.TotalReviews, sum);
    }

    [Fact]
    public void RecalculateRating_WithZeroReviews_Succeeds()
    {
        var aggregate = CreateExistingAggregate();

        var result = aggregate.RecalculateRating(
            oneStar: 0, twoStar: 0, threeStar: 0, fourStar: 0, fiveStar: 0,
            verifiedReviews: 0,
            totalVerifiedRatingSum: 0,
            totalUnverifiedRatingSum: 0);

        Assert.True(result.IsSuccess);
        Assert.Equal(0m, result.Model.Root.AverageRating);
        Assert.Equal("Pending", result.Model.Root.Status);
    }
}
