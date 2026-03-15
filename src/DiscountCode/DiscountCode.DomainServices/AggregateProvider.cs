namespace DiscountCode.DomainServices;

using DiscountCode.Domain.Abstraction;

public sealed class AggregateProvider
{
    public object Create(IDiscountCodeAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
