using Hive.SeedWorks.TacticalPatterns;

namespace Session.Domain.Abstraction;

/// <summary>
/// Aggregate root for the Session bounded context.
/// </summary>
public interface ISessionRoot : IAggregateRoot<ISession>
{
    Guid CustomerId { get; }
    string Token { get; }
    DateTime ExpiresAt { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
    string DeviceInfo { get; }
}
