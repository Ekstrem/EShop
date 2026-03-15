namespace StockItem.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using StockItem.Domain;
using StockItem.Domain.Abstraction;

public sealed record AdjustStockCommand(
    Guid StockItemId,
    int NewTotal)
    : IRequest<AggregateResult<IStockItem, IStockItemAnemicModel>>;
