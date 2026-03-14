namespace AggregateRating.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using AggregateRating.Domain;
using AggregateRating.Domain.Abstraction;
using AggregateRating.Domain.Implementation;
using AggregateRating.InternalContracts;

public sealed class RecalculateRatingHandler
    : IRequestHandler<RecalculateRatingCommand, AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>>
{
    private readonly IAggregateProvider<IAggregateRating, IAggregateRatingAnemicModel> _provider;
    private readonly IAggregateRatingQueryRepository _queryRepository;

    public RecalculateRatingHandler(
        IAggregateProvider<IAggregateRating, IAggregateRatingAnemicModel> provider,
        IAggregateRatingQueryRepository queryRepository)
    {
        _provider = provider;
        _queryRepository = queryRepository;
    }

    public async Task<AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>> Handle(
        RecalculateRatingCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _queryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        if (existing is null)
            return AggregateResult<IAggregateRating, IAggregateRatingAnemicModel>
                .Fail("Aggregate rating not found for this product.");

        var root = AggregateRatingRoot.CreateInstance(
            existing.Id,
            existing.ProductId,
            existing.AverageRating,
            existing.TotalReviews,
            existing.VerifiedReviews,
            existing.Status);

        var currentModel = new AggregateRatingAnemicModel
        {
            Root = root,
            Distribution = RatingDistribution.CreateInstance(
                existing.Distribution.OneStar,
                existing.Distribution.TwoStar,
                existing.Distribution.ThreeStar,
                existing.Distribution.FourStar,
                existing.Distribution.FiveStar),
            WeightedAverage = existing.WeightedAverage
        };

        var aggregate = AggregateRatingAggregate.CreateInstance(currentModel);

        return aggregate.RecalculateRating(
            request.OneStar,
            request.TwoStar,
            request.ThreeStar,
            request.FourStar,
            request.FiveStar,
            request.VerifiedReviews,
            request.TotalVerifiedRatingSum,
            request.TotalUnverifiedRatingSum);
    }
}
