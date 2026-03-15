using Hive.SeedWorks.TacticalPatterns;

namespace AggregateRating.Domain.Abstraction;

/// <summary>
/// Aggregate root for the AggregateRating bounded context.
/// </summary>
public interface IAggregateRatingRoot : IAggregateRoot<IAggregateRating>
{
    Guid Id { get; }
    Guid ProductId { get; }
    decimal AverageRating { get; }
    int TotalReviews { get; }
    int VerifiedReviews { get; }
    string Status { get; }
}
