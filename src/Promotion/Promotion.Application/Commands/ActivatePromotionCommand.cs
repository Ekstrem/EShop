namespace Promotion.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Promotion.Domain;
using Promotion.Domain.Abstraction;

public sealed record ActivatePromotionCommand(Guid PromotionId)
    : IRequest<AggregateResult<IPromotion, IPromotionAnemicModel>>;
