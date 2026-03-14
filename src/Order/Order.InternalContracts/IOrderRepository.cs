namespace Order.InternalContracts;

using Hive.SeedWorks.TacticalPatterns;
using Order.Domain;
using Order.Domain.Abstraction;

public interface IOrderRepository : IRepository<IOrder, IOrderAnemicModel>
{
    Task<IOrderAnemicModel?> GetByOrderNumberAsync(string orderNumber, CancellationToken ct = default);
}
