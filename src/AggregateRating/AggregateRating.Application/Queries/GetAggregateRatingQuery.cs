namespace AggregateRating.Application.Queries;

using MediatR;
using AggregateRating.InternalContracts;

public sealed record GetAggregateRatingQuery(Guid ProductId)
    : IRequest<AggregateRatingReadModel?>;
