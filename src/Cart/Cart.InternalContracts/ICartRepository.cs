namespace Cart.InternalContracts;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain;
using Cart.Domain.Abstraction;

public interface ICartRepository : IRepository<ICart, ICartAnemicModel>
{
    Task<ICartAnemicModel?> GetActiveCartByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
    Task<ICartAnemicModel?> GetActiveCartBySessionIdAsync(Guid sessionId, CancellationToken ct = default);
}
