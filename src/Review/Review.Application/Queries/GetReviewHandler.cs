namespace Review.Application.Queries;

using MediatR;
using Review.InternalContracts;

public sealed class GetReviewHandler : IRequestHandler<GetReviewQuery, ReviewReadModel?>
{
    private readonly IReviewQueryRepository _repository;

    public GetReviewHandler(IReviewQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReviewReadModel?> Handle(
        GetReviewQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.ReviewId, cancellationToken);
    }
}
