namespace AggregateRating.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using AggregateRating.Domain;
using AggregateRating.Domain.Abstraction;

public sealed class AggregateProvider : IAggregateProvider<IAggregateRating, IAggregateRatingAnemicModel>
{
    public IAggregate<IAggregateRating, IAggregateRatingAnemicModel> Create(IAggregateRatingAnemicModel model)
        => Domain.Implementation.AggregateRatingAggregate.CreateInstance(model);
}
