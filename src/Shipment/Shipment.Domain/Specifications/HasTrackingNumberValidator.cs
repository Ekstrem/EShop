using Hive.SeedWorks.TacticalPatterns;
using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that the shipment has a tracking number assigned.
/// </summary>
public sealed class HasTrackingNumberValidator : IBusinessOperationValidator<IShipment, IShipmentAnemicModel>
{
    public bool IsValid(IShipmentAnemicModel model)
        => !string.IsNullOrWhiteSpace(model.Root.TrackingNumber);

    public string ErrorMessage => "Shipment must have a tracking number assigned.";
}
