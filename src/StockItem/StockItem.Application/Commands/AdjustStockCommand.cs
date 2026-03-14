namespace StockItem.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using StockItem.Domain;
using StockItem.Domain.Abstraction;

public sealed record AdjustStockCommand(
    Guid StockItemId,
    int NewTotal)
    : IRequest<AggregateResult<IStockItem, IStockItemAnemicModel>>;
