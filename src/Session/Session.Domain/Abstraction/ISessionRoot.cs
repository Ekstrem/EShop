namespace Session.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

/// <summary>
/// Aggregate root for the Session bounded context.
/// </summary>
public interface ISessionRoot : IValueObject
{
    Guid Id { get; }
    Guid CustomerId { get; }
    string Token { get; }
    DateTime ExpiresAt { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
    string DeviceInfo { get; }
}
