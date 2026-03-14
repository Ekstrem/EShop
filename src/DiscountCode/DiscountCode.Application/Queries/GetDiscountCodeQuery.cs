namespace DiscountCode.Application.Queries;

using MediatR;
using DiscountCode.InternalContracts;

public sealed record GetDiscountCodeQuery(Guid DiscountCodeId)
    : IRequest<DiscountCodeReadModel?>;
