namespace StockItem.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using StockItem.Domain;
using StockItem.Domain.Abstraction;

public sealed record ReplenishStockCommand(
    Guid StockItemId,
    int Quantity)
    : IRequest<AggregateResult<IStockItem, IStockItemAnemicModel>>;
