namespace Session.InternalContracts;

/// <summary>
/// Read model projection for session queries.
/// </summary>
public sealed class SessionReadModel
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string DeviceInfo { get; init; } = string.Empty;
}
