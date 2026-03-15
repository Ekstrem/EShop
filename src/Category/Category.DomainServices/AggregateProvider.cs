namespace Category.DomainServices;

using Category.Domain.Abstraction;

public sealed class AggregateProvider
{
    public object Create(ICategoryAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
