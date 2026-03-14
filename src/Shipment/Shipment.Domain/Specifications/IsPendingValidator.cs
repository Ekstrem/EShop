using Hive.SeedWorks.TacticalPatterns;
using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment is in Pending status.
/// </summary>
public sealed class IsPendingValidator : IBusinessOperationValidator<IShipment, IShipmentAnemicModel>
{
    public bool IsValid(IShipmentAnemicModel model)
        => model.Root.Status == "Pending";

    public string ErrorMessage => "Shipment must be in Pending status.";
}
