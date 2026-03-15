namespace StockItem.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using StockItem.Domain;
using StockItem.Domain.Abstraction;

public sealed record ReserveStockCommand(
    Guid StockItemId,
    Guid OrderId,
    int Quantity)
    : IRequest<AggregateResult<IStockItem, IStockItemAnemicModel>>;
