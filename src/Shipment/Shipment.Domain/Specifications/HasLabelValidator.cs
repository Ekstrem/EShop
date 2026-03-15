using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment has a shipping label.
/// </summary>
internal sealed class HasLabelValidator
{
    public bool IsSatisfiedBy(IShipmentAnemicModel model)
        => model.Label is not null && !string.IsNullOrWhiteSpace(model.Label.LabelUrl);

    public string Reason => "Shipment must have a shipping label.";
}
