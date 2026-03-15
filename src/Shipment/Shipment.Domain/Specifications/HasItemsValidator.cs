using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment contains at least one item.
/// </summary>
internal sealed class HasItemsValidator
{
    public bool IsSatisfiedBy(IShipmentAnemicModel model)
        => model.Items.Count >= 1;

    public string Reason => "Shipment must contain at least one item.";
}
