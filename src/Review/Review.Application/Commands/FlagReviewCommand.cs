namespace Review.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed record FlagReviewCommand(
    Guid ReviewId,
    Guid FlaggerId,
    string Reason)
    : IRequest<AggregateResult<IReview, IReviewAnemicModel>>;
