namespace Cart.Storage;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Cart.Domain;
using Cart.Domain.Abstraction;
using Cart.InternalContracts;

public class CartRepository : ICartRepository
{
    // Placeholder for actual data access implementation (e.g., Postgres via CommandRepository)

    public Task<ICartAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task SaveAsync(AggregateResult<ICart, ICartAnemicModel> result, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<ICartAnemicModel?> GetActiveCartByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<ICartAnemicModel?> GetActiveCartBySessionIdAsync(Guid sessionId, CancellationToken ct = default)
        => throw new NotImplementedException();
}
