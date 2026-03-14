using Hive.SeedWorks.TacticalPatterns;

namespace Session.Domain;

/// <summary>
/// Bounded context marker for the Session context.
/// </summary>
public interface ISession : IBoundedContext
{
}

/// <summary>
/// Describes the Session bounded context.
/// </summary>
public sealed class SessionBoundedContextDescription : IBoundedContextDescription
{
    public string Name => "Session";

    public string Description =>
        "Manages authentication sessions, token lifecycle, and session security.";
}
