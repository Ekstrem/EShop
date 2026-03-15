using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment has a tracking number assigned.
/// </summary>
internal sealed class HasTrackingNumberValidator
{
    public bool IsSatisfiedBy(IShipmentAnemicModel model)
        => !string.IsNullOrWhiteSpace(model.Root.TrackingNumber);

    public string Reason => "Shipment must have a tracking number assigned.";
}
