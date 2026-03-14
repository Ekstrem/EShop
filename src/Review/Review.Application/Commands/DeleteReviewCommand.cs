namespace Review.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed record DeleteReviewCommand(
    Guid ReviewId,
    Guid RequesterId)
    : IRequest<AggregateResult<IReview, IReviewAnemicModel>>;
