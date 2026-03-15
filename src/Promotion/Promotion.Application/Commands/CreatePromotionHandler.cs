namespace Promotion.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Promotion.Domain;
using Promotion.Domain.Abstraction;
using Promotion.Domain.Implementation;

public sealed class CreatePromotionHandler
    : IRequestHandler<CreatePromotionCommand, AggregateResult<IPromotion, IPromotionAnemicModel>>
{
    public Task<AggregateResult<IPromotion, IPromotionAnemicModel>> Handle(
        CreatePromotionCommand request,
        CancellationToken cancellationToken)
    {
        var emptyModel = new AnemicModel();
        var aggregate = Aggregate.CreateInstance(emptyModel);
        var result = aggregate.CreatePromotion(
            request.Name,
            request.Description,
            request.DiscountType,
            request.DiscountValue,
            request.StartDate,
            request.EndDate,
            request.Conditions,
            request.AllowStacking);

        return Task.FromResult(result);
    }
}
