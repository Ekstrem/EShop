using Hive.SeedWorks.TacticalPatterns;
using AggregateRating.Domain.Abstraction;

namespace AggregateRating.Domain.Implementation;

/// <summary>
/// Immutable anemic model for the AggregateRating aggregate.
/// </summary>
public sealed class AggregateRatingAnemicModel : AnemicModel<IAggregateRating>, IAggregateRatingAnemicModel
{
    public IAggregateRatingRoot Root { get; set; } = null!;
    public IRatingDistribution Distribution { get; set; } = RatingDistribution.Empty();
    public decimal WeightedAverage { get; set; }
}
