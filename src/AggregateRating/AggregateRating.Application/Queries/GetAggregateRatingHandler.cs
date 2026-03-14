namespace AggregateRating.Application.Queries;

using MediatR;
using AggregateRating.InternalContracts;

public sealed class GetAggregateRatingHandler
    : IRequestHandler<GetAggregateRatingQuery, AggregateRatingReadModel?>
{
    private readonly IAggregateRatingQueryRepository _repository;

    public GetAggregateRatingHandler(IAggregateRatingQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<AggregateRatingReadModel?> Handle(
        GetAggregateRatingQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByProductIdAsync(request.ProductId, cancellationToken);
    }
}
