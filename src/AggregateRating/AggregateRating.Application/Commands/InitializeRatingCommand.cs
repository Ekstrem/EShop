namespace AggregateRating.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using AggregateRating.Domain;
using AggregateRating.Domain.Abstraction;

public sealed record InitializeRatingCommand(Guid ProductId)
    : IRequest<AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>>;
