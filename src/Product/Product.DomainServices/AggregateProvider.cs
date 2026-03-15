namespace Product.DomainServices;

using Product.Domain.Abstraction;

public sealed class AggregateProvider
{
    public object Create(IProductAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
