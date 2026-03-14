using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Implementation;

/// <summary>
/// Immutable implementation of the shipping label value object.
/// </summary>
internal sealed class ShippingLabel : IShippingLabel
{
    public string LabelUrl { get; private set; } = string.Empty;
    public DateTime GeneratedAt { get; private set; }

    private ShippingLabel() { }

    public static IShippingLabel CreateInstance(
        string labelUrl,
        DateTime generatedAt)
        => new ShippingLabel
        {
            LabelUrl = labelUrl,
            GeneratedAt = generatedAt
        };
}
