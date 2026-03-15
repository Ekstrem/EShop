namespace AggregateRating.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IRatingDistribution : IValueObject
{
    int OneStar { get; }
    int TwoStar { get; }
    int ThreeStar { get; }
    int FourStar { get; }
    int FiveStar { get; }
}
