using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment is in Packed status.
/// </summary>
internal sealed class IsPackedValidator
{
    public bool IsSatisfiedBy(IShipmentAnemicModel model)
        => model.Root.Status == "Packed";

    public string Reason => "Shipment must be in Packed status.";
}
