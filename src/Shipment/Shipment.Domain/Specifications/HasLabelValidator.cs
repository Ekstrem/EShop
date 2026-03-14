using Hive.SeedWorks.TacticalPatterns;
using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment has a shipping label.
/// </summary>
public sealed class HasLabelValidator : IBusinessOperationValidator<IShipment, IShipmentAnemicModel>
{
    public bool IsValid(IShipmentAnemicModel model)
        => model.Label is not null && !string.IsNullOrWhiteSpace(model.Label.LabelUrl);

    public string ErrorMessage => "Shipment must have a shipping label.";
}
