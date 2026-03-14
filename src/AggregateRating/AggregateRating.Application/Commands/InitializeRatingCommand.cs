namespace AggregateRating.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using AggregateRating.Domain;
using AggregateRating.Domain.Abstraction;

public sealed record InitializeRatingCommand(Guid ProductId)
    : IRequest<AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>>;
