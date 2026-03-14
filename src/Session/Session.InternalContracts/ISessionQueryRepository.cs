namespace Session.InternalContracts;

/// <summary>
/// Query repository contract for reading session projections.
/// </summary>
public interface ISessionQueryRepository
{
    Task<SessionReadModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<SessionReadModel?> GetByTokenAsync(string token, CancellationToken ct = default);
    Task<IReadOnlyList<SessionReadModel>> GetActiveByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
    Task<int> GetActiveSessionCountAsync(Guid customerId, CancellationToken ct = default);
    Task<int> GetFailedLoginAttemptsAsync(Guid customerId, TimeSpan window, CancellationToken ct = default);
    Task<DateTime?> GetLastFailedLoginAttemptAsync(Guid customerId, CancellationToken ct = default);
}
