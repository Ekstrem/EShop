namespace AggregateRating.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using AggregateRating.Domain;
using AggregateRating.Domain.Abstraction;
using AggregateRating.Domain.Implementation;

public sealed class InitializeRatingHandler
    : IRequestHandler<InitializeRatingCommand, AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>>
{
    private readonly IAggregateProvider<IAggregateRating, IAggregateRatingAnemicModel> _provider;

    public InitializeRatingHandler(
        IAggregateProvider<IAggregateRating, IAggregateRatingAnemicModel> provider)
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
