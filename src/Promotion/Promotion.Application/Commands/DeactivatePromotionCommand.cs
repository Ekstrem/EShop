namespace Promotion.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Promotion.Domain;
using Promotion.Domain.Abstraction;

public sealed record DeactivatePromotionCommand(Guid PromotionId)
    : IRequest<AggregateResult<IPromotion, IPromotionAnemicModel>>;
