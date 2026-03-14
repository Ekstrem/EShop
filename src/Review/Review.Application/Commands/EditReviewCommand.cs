namespace Review.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed record EditReviewCommand(
    Guid ReviewId,
    Guid RequesterId,
    int Rating,
    string Title,
    string Text)
    : IRequest<AggregateResult<IReview, IReviewAnemicModel>>;
