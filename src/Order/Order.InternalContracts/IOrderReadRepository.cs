namespace Order.InternalContracts;

using Order.Domain.Abstraction;

public interface IOrderReadRepository
{
    Task<IOrderAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IOrderAnemicModel?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default);
    Task<IReadOnlyList<IOrderAnemicModel>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
}
