namespace AggregateRating.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using AggregateRating.Domain.Abstraction;

internal sealed class AggregateRatingAnemicModel : IAggregateRatingAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public IAggregateRatingRoot Root { get; set; } = null!;
    public IRatingDistribution Distribution { get; set; } = RatingDistribution.Empty();
    public decimal WeightedAverage { get; set; }

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
