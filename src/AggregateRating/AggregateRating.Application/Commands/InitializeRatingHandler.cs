namespace AggregateRating.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using EShop.Contracts;
using AggregateRating.Domain;
using AggregateRating.Domain.Abstraction;
using AggregateRating.Domain.Implementation;
using AggregateRating.DomainServices;

public sealed class InitializeRatingHandler
    : IRequestHandler<InitializeRatingCommand, AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>>
{
    private readonly AggregateProvider _provider;

    public InitializeRatingHandler(AggregateProvider provider)
    {
        _provider = provider;
    }

    public Task<AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>> Handle(
        InitializeRatingCommand request,
        CancellationToken cancellationToken)
    {
        var emptyModel = new AggregateRatingAnemicModel();
        var aggregate = AggregateRatingAggregate.CreateInstance(emptyModel);
        var result = aggregate.InitializeRating(request.ProductId);

        return Task.FromResult(result);
    }
}
