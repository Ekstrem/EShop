namespace Product.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Product.Domain;
using Product.Domain.Abstraction;

public sealed record ArchiveProductCommand(Guid ProductId)
    : IRequest<AggregateResult<IProduct, IProductAnemicModel>>;
