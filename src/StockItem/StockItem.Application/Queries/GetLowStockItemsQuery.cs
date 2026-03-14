namespace StockItem.Application.Queries;

using MediatR;
using StockItem.InternalContracts;

public sealed record GetLowStockItemsQuery()
    : IRequest<IReadOnlyList<StockItemReadModel>>;

public sealed class GetLowStockItemsHandler
    : IRequestHandler<GetLowStockItemsQuery, IReadOnlyList<StockItemReadModel>>
{
    private readonly IStockItemQueryRepository _repository;

    public GetLowStockItemsHandler(IStockItemQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<StockItemReadModel>> Handle(
        GetLowStockItemsQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetLowStockItemsAsync(cancellationToken);
    }
}
