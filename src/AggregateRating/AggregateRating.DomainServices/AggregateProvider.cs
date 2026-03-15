namespace AggregateRating.DomainServices;

using AggregateRating.Domain.Abstraction;

public sealed class AggregateProvider
{
    public object Create(IAggregateRatingAnemicModel model)
        => Domain.Implementation.AggregateRatingAggregate.CreateInstance(model);
}
