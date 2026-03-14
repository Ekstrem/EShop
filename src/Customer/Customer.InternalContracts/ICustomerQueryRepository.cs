namespace Customer.InternalContracts;

/// <summary>
/// Query repository contract for reading customer projections.
/// </summary>
public interface ICustomerQueryRepository
{
    Task<CustomerReadModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CustomerReadModel?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
}
