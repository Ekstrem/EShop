using AggregateRating.Domain.Abstraction;

namespace AggregateRating.Domain.Implementation;

/// <summary>
/// Immutable implementation of the AggregateRating aggregate root.
/// </summary>
public sealed class AggregateRatingRoot : IAggregateRatingRoot
{
    private AggregateRatingRoot(
        Guid id,
        Guid productId,
        decimal averageRating,
        int totalReviews,
        int verifiedReviews,
        string status)
    {
        Id = id;
        ProductId = productId;
        AverageRating = averageRating;
        TotalReviews = totalReviews;
        VerifiedReviews = verifiedReviews;
        Status = status;
    }

    public Guid Id { get; }
    public Guid ProductId { get; }
    public decimal AverageRating { get; }
    public int TotalReviews { get; }
    public int VerifiedReviews { get; }
    public string Status { get; }

    public static AggregateRatingRoot CreateInstance(
        Guid id,
        Guid productId,
        decimal averageRating,
        int totalReviews,
        int verifiedReviews,
        string status)
    {
        return new AggregateRatingRoot(id, productId, averageRating, totalReviews, verifiedReviews, status);
    }
}
