using Hive.SeedWorks.TacticalPatterns;

namespace ReturnRequest.Domain.Abstraction;

/// <summary>
/// Value object representing an item within a return request.
/// </summary>
public interface IReturnItem : IValueObject
{
    Guid VariantId { get; }
    string ProductName { get; }
    int Quantity { get; }
    decimal UnitPrice { get; }
}
