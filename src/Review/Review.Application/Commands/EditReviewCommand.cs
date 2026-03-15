namespace Review.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed record EditReviewCommand(
    Guid ReviewId,
    Guid RequesterId,
    int Rating,
    string Title,
    string Text)
    : IRequest<AggregateResult<IReview, IReviewAnemicModel>>;
