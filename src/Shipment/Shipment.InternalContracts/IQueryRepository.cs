using Hive.SeedWorks.TacticalPatterns;

namespace Shipment.InternalContracts;

/// <summary>
/// Base query repository contract.
/// </summary>
public interface IQueryRepository<TBoundedContext> where TBoundedContext : IBoundedContext
{
}
