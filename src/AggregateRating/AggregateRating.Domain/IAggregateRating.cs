using Hive.SeedWorks.TacticalPatterns;

namespace AggregateRating.Domain;

/// <summary>
/// Bounded context marker for the AggregateRating context.
/// </summary>
public interface IAggregateRating : IBoundedContext
{
}

/// <summary>
/// Describes the AggregateRating bounded context.
/// </summary>
public sealed class AggregateRatingBoundedContextDescription : IBoundedContextDescription
{
    public string Name => "AggregateRating";

    public string Description =>
        "Manages aggregate product ratings, distribution statistics, and weighted averages.";
}
