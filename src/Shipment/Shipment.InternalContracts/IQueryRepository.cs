using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;

namespace Shipment.InternalContracts;

/// <summary>
/// Base query repository contract.
/// </summary>
public interface IQueryRepository<TBoundedContext> where TBoundedContext : IBoundedContext
{
}
