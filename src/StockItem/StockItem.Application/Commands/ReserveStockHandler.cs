namespace StockItem.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using EShop.Contracts;
using StockItem.Domain;
using StockItem.Domain.Abstraction;
using StockItem.Domain.Implementation;
using StockItem.DomainServices;

public sealed class ReserveStockHandler
    : IRequestHandler<ReserveStockCommand, AggregateResult<IStockItem, IStockItemAnemicModel>>
{
    private readonly AggregateProvider _provider;

    public ReserveStockHandler(AggregateProvider provider)
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
            AggregateResultExtensions.FailResult<IStockItem, IStockItemAnemicModel>("Not yet wired to repository."));
    }
}
