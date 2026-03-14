namespace DiscountCode.Application.Queries;

using MediatR;
using DiscountCode.InternalContracts;

public sealed record SearchDiscountCodesQuery(
    string? Code,
    Guid? PromotionId,
    string? Status,
    int Skip,
    int Take)
    : IRequest<IReadOnlyList<DiscountCodeReadModel>>;
