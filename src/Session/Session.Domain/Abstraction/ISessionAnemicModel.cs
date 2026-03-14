using Hive.SeedWorks.TacticalPatterns;

namespace Session.Domain.Abstraction;

/// <summary>
/// Anemic model contract for the Session aggregate.
/// </summary>
public interface ISessionAnemicModel : IAnemicModel<ISession>
{
    ISessionRoot Root { get; }
}
