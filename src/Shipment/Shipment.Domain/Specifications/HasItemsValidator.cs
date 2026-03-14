using Hive.SeedWorks.TacticalPatterns;
using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment contains at least one item.
/// </summary>
public sealed class HasItemsValidator : IBusinessOperationValidator<IShipment, IShipmentAnemicModel>
{
    public bool IsValid(IShipmentAnemicModel model)
        => model.Items.Count >= 1;

    public string ErrorMessage => "Shipment must contain at least one item.";
}
