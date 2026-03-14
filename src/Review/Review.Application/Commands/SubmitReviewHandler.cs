namespace Review.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Review.Domain;
using Review.Domain.Abstraction;
using Review.Domain.Implementation;
using Review.InternalContracts;

public sealed class SubmitReviewHandler
    : IRequestHandler<SubmitReviewCommand, AggregateResult<IReview, IReviewAnemicModel>>
{
    private readonly IAggregateProvider<IReview, IReviewAnemicModel> _provider;
    private readonly IReviewQueryRepository _queryRepository;

    public SubmitReviewHandler(
        IAggregateProvider<IReview, IReviewAnemicModel> provider,
        IReviewQueryRepository queryRepository)
    {
        _provider = provider;
        _queryRepository = queryRepository;
    }

    public async Task<AggregateResult<IReview, IReviewAnemicModel>> Handle(
        SubmitReviewCommand request,
        CancellationToken cancellationToken)
    {
        var aggregate = ReviewAggregate.CreateInstance(new ReviewAnemicModel());
        var result = aggregate.SubmitReview(
            request.ProductId,
            request.CustomerId,
            request.Rating,
            request.Title,
            request.Text,
            request.IsVerifiedPurchase,
            (customerId, productId) =>
            {
                var existing = _queryRepository
                    .GetByCustomerAndProductAsync(customerId, productId, cancellationToken)
                    .GetAwaiter().GetResult();
                return existing is not null;
            });

        return result;
    }
}
