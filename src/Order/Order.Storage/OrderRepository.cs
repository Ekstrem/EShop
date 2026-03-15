namespace Order.Storage;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Order.Domain;
using Order.Domain.Abstraction;
using Order.InternalContracts;

public class OrderRepository : IOrderRepository
{
    // Placeholder for actual data access implementation (e.g., Postgres via CommandRepository)

    public Task<IOrderAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task SaveAsync(AggregateResult<IOrder, IOrderAnemicModel> result, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IOrderAnemicModel?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default)
        => throw new NotImplementedException();
}
