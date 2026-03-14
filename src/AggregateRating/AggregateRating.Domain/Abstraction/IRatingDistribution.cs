using Hive.SeedWorks.TacticalPatterns;

namespace AggregateRating.Domain.Abstraction;

/// <summary>
/// Value object representing the distribution of ratings across star levels.
/// </summary>
public interface IRatingDistribution : IValueObject
{
    int OneStar { get; }
    int TwoStar { get; }
    int ThreeStar { get; }
    int FourStar { get; }
    int FiveStar { get; }
}
