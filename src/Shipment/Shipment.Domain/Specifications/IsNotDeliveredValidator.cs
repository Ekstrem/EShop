using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment has not yet been delivered.
/// </summary>
internal sealed class IsNotDeliveredValidator
{
    public bool IsSatisfiedBy(IShipmentAnemicModel model)
        => model.Root.Status != "Delivered";

    public string Reason => "Shipment has already been delivered.";
}
