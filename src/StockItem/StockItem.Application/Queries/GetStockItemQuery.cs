namespace StockItem.Application.Queries;

using MediatR;
using StockItem.InternalContracts;

public sealed record GetStockItemQuery(Guid StockItemId)
    : IRequest<StockItemReadModel?>;

public sealed class GetStockItemHandler : IRequestHandler<GetStockItemQuery, StockItemReadModel?>
{
    private readonly IStockItemQueryRepository _repository;

    public GetStockItemHandler(IStockItemQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<StockItemReadModel?> Handle(
        GetStockItemQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.StockItemId, cancellationToken);
    }
}
