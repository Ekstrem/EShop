using Hive.SeedWorks.TacticalPatterns;

namespace Review.Domain.Abstraction;

/// <summary>
/// Value object representing a helpful vote on a review.
/// </summary>
public interface IHelpfulVote : IValueObject
{
    Guid VoterId { get; }
    DateTime VotedAt { get; }
}
