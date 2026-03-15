namespace Review.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed record RespondToReviewCommand(
    Guid ReviewId,
    string ModeratorResponse)
    : IRequest<AggregateResult<IReview, IReviewAnemicModel>>;
