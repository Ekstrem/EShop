namespace Session.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

/// <summary>
/// Anemic model contract for the Session aggregate.
/// </summary>
public interface ISessionAnemicModel : IAnemicModel<ISession>
{
    ISessionRoot Root { get; }
}
