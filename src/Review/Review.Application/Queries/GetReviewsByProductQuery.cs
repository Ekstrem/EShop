namespace Review.Application.Queries;

using MediatR;
using Review.InternalContracts;

public sealed record GetReviewsByProductQuery(
    Guid ProductId,
    string? Status,
    int Skip = 0,
    int Take = 20)
    : IRequest<IReadOnlyList<ReviewReadModel>>;

public sealed class GetReviewsByProductHandler
    : IRequestHandler<GetReviewsByProductQuery, IReadOnlyList<ReviewReadModel>>
{
    private readonly IReviewQueryRepository _repository;

    public GetReviewsByProductHandler(IReviewQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ReviewReadModel>> Handle(
        GetReviewsByProductQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByProductIdAsync(
            request.ProductId,
            request.Status,
            request.Skip,
            request.Take,
            cancellationToken);
    }
}
