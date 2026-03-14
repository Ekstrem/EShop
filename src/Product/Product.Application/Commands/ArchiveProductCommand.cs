namespace Product.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Product.Domain;
using Product.Domain.Abstraction;

public sealed record ArchiveProductCommand(Guid ProductId)
    : IRequest<AggregateResult<IProduct, IProductAnemicModel>>;
