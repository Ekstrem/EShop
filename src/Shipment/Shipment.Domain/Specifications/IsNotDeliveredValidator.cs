using Hive.SeedWorks.TacticalPatterns;
using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment has not yet been delivered.
/// </summary>
public sealed class IsNotDeliveredValidator : IBusinessOperationValidator<IShipment, IShipmentAnemicModel>
{
    public bool IsValid(IShipmentAnemicModel model)
        => model.Root.Status != "Delivered";

    public string ErrorMessage => "Shipment has already been delivered.";
}
