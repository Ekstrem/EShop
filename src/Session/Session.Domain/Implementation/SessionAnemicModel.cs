using Hive.SeedWorks.TacticalPatterns;
using Session.Domain.Abstraction;

namespace Session.Domain.Implementation;

/// <summary>
/// Immutable anemic model for the Session aggregate.
/// </summary>
public sealed class SessionAnemicModel : AnemicModel<ISession>, ISessionAnemicModel
{
    private SessionAnemicModel(Guid id, ISessionRoot root)
    {
        Id = id;
        Root = root;
    }

    public Guid Id { get; }
    public ISessionRoot Root { get; }

    public static SessionAnemicModel CreateInstance(Guid id, ISessionRoot root)
    {
        return new SessionAnemicModel(id, root);
    }
}
