namespace StockItem.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using StockItem.Domain;
using StockItem.Domain.Abstraction;

public sealed record ReleaseStockCommand(
    Guid StockItemId,
    Guid OrderId)
    : IRequest<AggregateResult<IStockItem, IStockItemAnemicModel>>;
