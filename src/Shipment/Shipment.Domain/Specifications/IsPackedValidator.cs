using Hive.SeedWorks.TacticalPatterns;
using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment is in Packed status.
/// </summary>
public sealed class IsPackedValidator : IBusinessOperationValidator<IShipment, IShipmentAnemicModel>
{
    public bool IsSatisfiedBy(IShipmentAnemicModel model)
        => model.Root.Status == "Packed";

    public string ErrorMessage => "Shipment must be in Packed status.";
}
