namespace AggregateRating.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IAggregateRatingRoot : IValueObject
{
    Guid Id { get; }
    Guid ProductId { get; }
    decimal AverageRating { get; }
    int TotalReviews { get; }
    int VerifiedReviews { get; }
    string Status { get; }
}
