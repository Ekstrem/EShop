using Hive.SeedWorks.TacticalPatterns;

namespace Shipment.Domain.Abstraction;

/// <summary>
/// Value object representing a shipping label.
/// </summary>
public interface IShippingLabel : IValueObject
{
    string LabelUrl { get; }
    DateTime GeneratedAt { get; }
}
