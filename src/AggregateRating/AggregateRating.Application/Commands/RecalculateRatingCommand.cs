namespace AggregateRating.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using AggregateRating.Domain;
using AggregateRating.Domain.Abstraction;

public sealed record RecalculateRatingCommand(
    Guid ProductId,
    int OneStar,
    int TwoStar,
    int ThreeStar,
    int FourStar,
    int FiveStar,
    int VerifiedReviews,
    int TotalVerifiedRatingSum,
    int TotalUnverifiedRatingSum)
    : IRequest<AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>>;
