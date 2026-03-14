namespace Product.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Product.Domain;
using Product.Domain.Abstraction;

public sealed record UpdateProductCommand(
    Guid ProductId,
    string Name,
    string Description,
    Guid CategoryId)
    : IRequest<AggregateResult<IProduct, IProductAnemicModel>>;
