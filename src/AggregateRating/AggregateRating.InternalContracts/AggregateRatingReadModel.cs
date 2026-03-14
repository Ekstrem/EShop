namespace AggregateRating.InternalContracts;

public sealed class AggregateRatingReadModel
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public decimal AverageRating { get; init; }
    public int TotalReviews { get; init; }
    public int VerifiedReviews { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal WeightedAverage { get; init; }
    public RatingDistributionReadModel Distribution { get; init; } = new();
}

public sealed class RatingDistributionReadModel
{
    public int OneStar { get; init; }
    public int TwoStar { get; init; }
    public int ThreeStar { get; init; }
    public int FourStar { get; init; }
    public int FiveStar { get; init; }
}
