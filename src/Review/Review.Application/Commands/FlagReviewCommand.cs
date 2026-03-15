namespace Review.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed record FlagReviewCommand(
    Guid ReviewId,
    Guid FlaggerId,
    string Reason)
    : IRequest<AggregateResult<IReview, IReviewAnemicModel>>;
