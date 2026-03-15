using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace ReturnRequest.Domain.Abstraction;

/// <summary>
/// Aggregate root for the ReturnRequest bounded context.
/// </summary>
public interface IReturnRequestRoot : IValueObject
{
    Guid Id { get; }
    Guid OrderId { get; }
    Guid CustomerId { get; }
    string RmaNumber { get; }
    string Reason { get; }
    string Status { get; }
    DateTime RequestedAt { get; }
}
