using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Implementation;

/// <summary>
/// Anemic model implementation for the Shipment aggregate.
/// </summary>
internal sealed class AnemicModel : IShipmentAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public IShipmentRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IShipmentItem> Items { get; internal set; } = new List<IShipmentItem>();
    public IShippingLabel? Label { get; internal set; }

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
