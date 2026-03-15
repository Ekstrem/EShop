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
    public string ContextName => "Review";
    public int MicroserviceVersion => 1;
}
