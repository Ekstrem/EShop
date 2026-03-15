using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment is in Pending status.
/// </summary>
internal sealed class IsPendingValidator
{
    public bool IsSatisfiedBy(IShipmentAnemicModel model)
        => model.Root.Status == "Pending";

    public string Reason => "Shipment must be in Pending status.";
}
