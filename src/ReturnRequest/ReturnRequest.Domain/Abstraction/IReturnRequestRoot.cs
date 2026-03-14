using Hive.SeedWorks.TacticalPatterns;

namespace ReturnRequest.Domain.Abstraction;

/// <summary>
/// Aggregate root for the ReturnRequest bounded context.
/// </summary>
public interface IReturnRequestRoot : IAggregateRoot<IReturnRequest>
{
    Guid OrderId { get; }
    Guid CustomerId { get; }
    string RmaNumber { get; }
    string Reason { get; }
    string Status { get; }
    DateTime RequestedAt { get; }
}
