namespace Review.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed record VoteHelpfulCommand(
    Guid ReviewId,
    Guid VoterId)
    : IRequest<AggregateResult<IReview, IReviewAnemicModel>>;
