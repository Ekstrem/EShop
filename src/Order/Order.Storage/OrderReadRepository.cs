namespace Order.Storage;

using Order.Domain.Abstraction;
using Order.InternalContracts;

public class OrderReadRepository : IOrderReadRepository
{
    // Placeholder for actual read data access implementation (e.g., Postgres via ReadRepository)

    public Task<IOrderAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IOrderAnemicModel?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IReadOnlyList<IOrderAnemicModel>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
        => throw new NotImplementedException();
}
