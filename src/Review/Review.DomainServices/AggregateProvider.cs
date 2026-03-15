namespace Review.DomainServices;

using Review.Domain.Abstraction;

public sealed class AggregateProvider
{
    public object Create(IReviewAnemicModel model)
        => Domain.Implementation.ReviewAggregate.CreateInstance(model);
}
