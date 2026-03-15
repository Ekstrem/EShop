namespace AggregateRating.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IAggregateRatingAnemicModel : IAnemicModel<IAggregateRating>
{
    IAggregateRatingRoot Root { get; }
    IRatingDistribution Distribution { get; }
    decimal WeightedAverage { get; }
}
