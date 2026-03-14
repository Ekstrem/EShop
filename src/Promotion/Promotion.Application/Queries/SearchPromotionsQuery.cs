namespace Promotion.Application.Queries;

using MediatR;
using Promotion.InternalContracts;

public sealed record SearchPromotionsQuery(
    string? Name,
    string? DiscountType,
    string? Status,
    int Skip,
    int Take)
    : IRequest<IReadOnlyList<PromotionReadModel>>;
