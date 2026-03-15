namespace Product.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Product.Domain;
using Product.Domain.Abstraction;

public sealed record UpdateProductCommand(
    Guid ProductId,
    string Name,
    string Description,
    Guid CategoryId)
    : IRequest<AggregateResult<IProduct, IProductAnemicModel>>;
