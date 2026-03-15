namespace DiscountCode.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DiscountCode.Domain;
using DiscountCode.Domain.Abstraction;
using DiscountCode.Domain.Implementation;

public sealed class GenerateDiscountCodeHandler
    : IRequestHandler<GenerateDiscountCodeCommand, AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>>
{
    public Task<AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>> Handle(
        GenerateDiscountCodeCommand request,
        CancellationToken cancellationToken)
    {
        var emptyModel = new AnemicModel();
        var aggregate = Aggregate.CreateInstance(emptyModel);
        var result = aggregate.GenerateDiscountCode(
            request.Code,
            request.PromotionId,
            request.MaxUsage,
            request.ExpiresAt);

        return Task.FromResult(result);
    }
}
