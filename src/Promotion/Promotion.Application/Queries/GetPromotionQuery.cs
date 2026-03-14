namespace Promotion.Application.Queries;

using MediatR;
using Promotion.InternalContracts;

public sealed record GetPromotionQuery(Guid PromotionId)
    : IRequest<PromotionReadModel?>;
