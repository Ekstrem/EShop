namespace Product.Application.Queries;

using MediatR;
using Product.InternalContracts;

public sealed record GetProductQuery(Guid ProductId)
    : IRequest<ProductReadModel?>;
