namespace Cart.InternalContracts;

using Cart.Domain.Abstraction;

public interface ICartReadRepository
{
    Task<ICartAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ICartAnemicModel?> GetActiveCartByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
    Task<IReadOnlyList<ICartAnemicModel>> GetCartsByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
}
