namespace Review.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed class AggregateProvider : IAggregateProvider<IReview, IReviewAnemicModel>
{
    public IAggregate<IReview, IReviewAnemicModel> Create(IReviewAnemicModel model)
        => Domain.Implementation.ReviewAggregate.CreateInstance(model);
}
