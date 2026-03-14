namespace Review.Application.Queries;

using MediatR;
using Review.InternalContracts;

public sealed record GetReviewQuery(Guid ReviewId)
    : IRequest<ReviewReadModel?>;
