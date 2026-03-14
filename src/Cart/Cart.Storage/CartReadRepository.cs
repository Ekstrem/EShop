namespace Cart.Storage;

using Cart.Domain.Abstraction;
using Cart.InternalContracts;

public class CartReadRepository : ICartReadRepository
{
    // Placeholder for actual read data access implementation (e.g., Postgres via ReadRepository)

    public Task<ICartAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<ICartAnemicModel?> GetActiveCartByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IReadOnlyList<ICartAnemicModel>> GetCartsByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
        => throw new NotImplementedException();
}
