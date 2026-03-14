using Hive.SeedWorks.TacticalPatterns;

namespace ReturnRequest.InternalContracts;

/// <summary>
/// Base query repository contract.
/// </summary>
public interface IQueryRepository<TBoundedContext> where TBoundedContext : IBoundedContext
{
}
