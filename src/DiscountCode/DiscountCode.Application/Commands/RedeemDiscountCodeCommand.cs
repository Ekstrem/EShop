namespace DiscountCode.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DiscountCode.Domain;
using DiscountCode.Domain.Abstraction;

public sealed record RedeemDiscountCodeCommand(
    Guid DiscountCodeId,
    Guid OrderId)
    : IRequest<AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>>;
