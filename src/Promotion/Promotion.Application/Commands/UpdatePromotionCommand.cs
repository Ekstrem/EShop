namespace Promotion.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Promotion.Domain;
using Promotion.Domain.Abstraction;

public sealed record UpdatePromotionCommand(
    Guid PromotionId,
    string Name,
    string Description,
    string DiscountType,
    decimal DiscountValue,
    DateTime StartDate,
    DateTime EndDate,
    string Conditions,
    bool AllowStacking)
    : IRequest<AggregateResult<IPromotion, IPromotionAnemicModel>>;
