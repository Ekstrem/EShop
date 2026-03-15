using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace Review.Domain.Abstraction;

/// <summary>
/// Aggregate root for the Review bounded context.
/// </summary>
public interface IReviewRoot : IValueObject
{
    Guid Id { get; }
    Guid ProductId { get; }
    Guid CustomerId { get; }
    int Rating { get; }
    string Title { get; }
    string Text { get; }
    bool IsVerifiedPurchase { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
    string? ModeratorResponse { get; }
}
