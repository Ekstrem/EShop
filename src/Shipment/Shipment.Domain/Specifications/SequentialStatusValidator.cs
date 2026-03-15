using Hive.SeedWorks.TacticalPatterns;
using Shipment.Domain.Abstraction;

namespace Shipment.Domain.Specifications;

/// <summary>
/// Validates that shipment status transitions follow the correct sequence:
/// Pending -> Packed -> Shipped -> InTransit -> Delivered.
/// </summary>
public sealed class SequentialStatusValidator : IBusinessOperationValidator<IShipment, IShipmentAnemicModel>
{
    private static readonly List<string> StatusOrder = new()
    {
        "Pending",
        "Packed",
        "Shipped",
        "InTransit",
        "Delivered"
    };

    private readonly string _targetStatus;

    public SequentialStatusValidator(string targetStatus)
    {
        _targetStatus = targetStatus;
    }

    public bool IsSatisfiedBy(IShipmentAnemicModel model)
    {
        var currentIndex = StatusOrder.IndexOf(model.Root.Status);
        var targetIndex = StatusOrder.IndexOf(_targetStatus);

        if (currentIndex < 0 || targetIndex < 0)
            return false;

        return targetIndex > currentIndex;
    }

    public string ErrorMessage =>
        $"Cannot transition to '{_targetStatus}' from current status. Status transitions must be sequential.";
}
