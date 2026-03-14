namespace DiscountCode.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using DiscountCode.Domain;
using DiscountCode.Domain.Abstraction;

public sealed record RedeemDiscountCodeCommand(
    Guid DiscountCodeId,
    Guid OrderId)
    : IRequest<AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>>;
