namespace Review.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed record RespondToReviewCommand(
    Guid ReviewId,
    string ModeratorResponse)
    : IRequest<AggregateResult<IReview, IReviewAnemicModel>>;
