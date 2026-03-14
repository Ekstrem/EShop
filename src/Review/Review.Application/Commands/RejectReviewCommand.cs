namespace Review.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed record RejectReviewCommand(Guid ReviewId)
    : IRequest<AggregateResult<IReview, IReviewAnemicModel>>;
