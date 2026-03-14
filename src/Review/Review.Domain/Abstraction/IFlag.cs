using Hive.SeedWorks.TacticalPatterns;

namespace Review.Domain.Abstraction;

/// <summary>
/// Value object representing a flag raised against a review.
/// </summary>
public interface IFlag : IValueObject
{
    Guid FlaggerId { get; }
    string Reason { get; }
    DateTime FlaggedAt { get; }
}
