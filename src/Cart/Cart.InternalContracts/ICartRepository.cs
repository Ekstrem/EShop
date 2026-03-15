namespace Cart.InternalContracts;

using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Cart.Domain;
using Cart.Domain.Abstraction;

public interface ICartRepository : IRepository<ICart, ICartAnemicModel>
{
    Task<ICartAnemicModel?> GetActiveCartByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
    Task<ICartAnemicModel?> GetActiveCartBySessionIdAsync(Guid sessionId, CancellationToken ct = default);
}
