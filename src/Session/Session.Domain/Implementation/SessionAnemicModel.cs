namespace Session.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Session.Domain.Abstraction;

/// <summary>
/// Immutable anemic model for the Session aggregate.
/// </summary>
public sealed class SessionAnemicModel : ISessionAnemicModel
{
    private SessionAnemicModel(Guid id, ISessionRoot root)
    {
        Id = id;
        Root = root;
    }

    public Guid Id { get; }
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public ISessionRoot Root { get; }

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };

    public static SessionAnemicModel CreateInstance(Guid id, ISessionRoot root)
    {
        return new SessionAnemicModel(id, root);
    }
}
