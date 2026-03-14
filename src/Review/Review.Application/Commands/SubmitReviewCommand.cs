namespace Review.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed record SubmitReviewCommand(
    Guid ProductId,
    Guid CustomerId,
    int Rating,
    string Title,
    string Text,
    bool IsVerifiedPurchase)
    : IRequest<AggregateResult<IReview, IReviewAnemicModel>>;
