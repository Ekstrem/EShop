namespace StockItem.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using StockItem.Domain;
using StockItem.Domain.Abstraction;
using StockItem.Domain.Implementation;

public sealed class ReserveStockHandler
    : IRequestHandler<ReserveStockCommand, AggregateResult<IStockItem, IStockItemAnemicModel>>
{
    private readonly IAggregateProvider<IStockItem, IStockItemAnemicModel> _provider;

    public ReserveStockHandler(IAggregateProvider<IStockItem, IStockItemAnemicModel> provider)
    {
        _provider = provider;
    }

    public Task<AggregateResult<IStockItem, IStockItemAnemicModel>> Handle(
        ReserveStockCommand request,
        CancellationToken cancellationToken)
    {
        // In real implementation, load model from repository
        // var model = await _repository.GetByIdAsync(request.StockItemId);
        // var aggregate = Aggregate.CreateInstance(model);
        // var result = aggregate.ReserveStock(request.OrderId, request.Quantity);
        // await _repository.SaveAsync(result.Value);
        return Task.FromResult(
            AggregateResult<IStockItem, IStockItemAnemicModel>.Fail("Not yet wired to repository."));
    }
}
