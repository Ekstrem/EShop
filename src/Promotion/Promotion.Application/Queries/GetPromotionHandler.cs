namespace Promotion.Application.Queries;

using MediatR;
using Promotion.InternalContracts;

public sealed class GetPromotionHandler : IRequestHandler<GetPromotionQuery, PromotionReadModel?>
{
    private readonly IPromotionQueryRepository _repository;

    public GetPromotionHandler(IPromotionQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<PromotionReadModel?> Handle(
        GetPromotionQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.PromotionId, cancellationToken);
    }
}
