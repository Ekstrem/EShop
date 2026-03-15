namespace Promotion.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Promotion.Domain;
using Promotion.Domain.Abstraction;

public sealed record CreatePromotionCommand(
    string Name,
    string Description,
    string DiscountType,
    decimal DiscountValue,
    DateTime StartDate,
    DateTime EndDate,
    string Conditions,
    bool AllowStacking)
    : IRequest<AggregateResult<IPromotion, IPromotionAnemicModel>>;
