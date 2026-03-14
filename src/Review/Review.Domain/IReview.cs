using Hive.SeedWorks.TacticalPatterns;

namespace Review.Domain;

/// <summary>
/// Bounded context marker for the Review context.
/// </summary>
public interface IReview : IBoundedContext
{
}

/// <summary>
/// Describes the Review bounded context.
/// </summary>
public sealed class ReviewBoundedContextDescription : IBoundedContextDescription
{
    public string Name => "Review";

    public string Description =>
        "Manages product reviews, ratings, moderation, helpful votes, and flagging.";
}
