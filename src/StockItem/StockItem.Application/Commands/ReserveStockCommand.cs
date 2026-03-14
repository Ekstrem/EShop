namespace StockItem.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using StockItem.Domain;
using StockItem.Domain.Abstraction;

public sealed record ReserveStockCommand(
    Guid StockItemId,
    Guid OrderId,
    int Quantity)
    : IRequest<AggregateResult<IStockItem, IStockItemAnemicModel>>;
