namespace DiscountCode.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using DiscountCode.Domain;
using DiscountCode.Domain.Abstraction;

public sealed record DeactivateCodeCommand(Guid DiscountCodeId)
    : IRequest<AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>>;
