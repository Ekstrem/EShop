using Session.Domain.Abstraction;

namespace Session.Domain.Implementation;

/// <summary>
/// Immutable implementation of the Session aggregate root.
/// </summary>
public sealed class SessionRoot : ISessionRoot
{
    private SessionRoot(
        Guid id,
        Guid customerId,
        string token,
        DateTime expiresAt,
        string status,
        DateTime createdAt,
        string deviceInfo)
    {
        Id = id;
        CustomerId = customerId;
        Token = token;
        ExpiresAt = expiresAt;
        Status = status;
        CreatedAt = createdAt;
        DeviceInfo = deviceInfo;
    }

    public Guid Id { get; }
    public Guid CustomerId { get; }
    public string Token { get; }
    public DateTime ExpiresAt { get; }
    public string Status { get; }
    public DateTime CreatedAt { get; }
    public string DeviceInfo { get; }

    public static SessionRoot CreateInstance(
        Guid id,
        Guid customerId,
        string token,
        DateTime expiresAt,
        string status,
        DateTime createdAt,
        string deviceInfo)
    {
        return new SessionRoot(id, customerId, token, expiresAt, status, createdAt, deviceInfo);
    }
}
