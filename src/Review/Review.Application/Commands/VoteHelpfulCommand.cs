namespace Review.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed record VoteHelpfulCommand(
    Guid ReviewId,
    Guid VoterId)
    : IRequest<AggregateResult<IReview, IReviewAnemicModel>>;
