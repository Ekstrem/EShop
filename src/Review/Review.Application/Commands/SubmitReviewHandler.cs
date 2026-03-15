namespace Review.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using EShop.Contracts;
using Review.Domain;
using Review.Domain.Abstraction;
using Review.Domain.Implementation;
using Review.DomainServices;
using Review.InternalContracts;

public sealed class SubmitReviewHandler
    : IRequestHandler<SubmitReviewCommand, AggregateResult<IReview, IReviewAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IReviewQueryRepository _queryRepository;

    public SubmitReviewHandler(
        AggregateProvider provider,
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
