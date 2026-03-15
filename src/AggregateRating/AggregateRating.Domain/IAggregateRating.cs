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
    public string ContextName => "AggregateRating";
    public int MicroserviceVersion => 1;
}
