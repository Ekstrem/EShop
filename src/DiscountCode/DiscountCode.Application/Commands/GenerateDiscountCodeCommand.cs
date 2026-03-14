namespace DiscountCode.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using DiscountCode.Domain;
using DiscountCode.Domain.Abstraction;

public sealed record GenerateDiscountCodeCommand(
    string Code,
    Guid? PromotionId,
    int MaxUsage,
    DateTime? ExpiresAt)
    : IRequest<AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>>;
