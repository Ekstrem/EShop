using Hive.SeedWorks.TacticalPatterns;

namespace AggregateRating.Domain.Abstraction;

/// <summary>
/// Anemic model contract for the AggregateRating aggregate.
/// </summary>
public interface IAggregateRatingAnemicModel : IAnemicModel<IAggregateRating>
{
    IAggregateRatingRoot Root { get; }
    IRatingDistribution Distribution { get; }
    decimal WeightedAverage { get; }
}
