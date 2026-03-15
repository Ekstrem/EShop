using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Implementation;
using Shipment.Domain.Specifications;
using Shipment.DomainServices;

namespace Shipment.Application.Commands;

/// <summary>
/// Handles the CreateShipmentCommand.
/// </summary>
public sealed class CreateShipmentCommandHandler
    : IRequestHandler<CreateShipmentCommand, AggregateResult<IShipment, IShipmentAnemicModel>>
{
    private readonly BusAdapter _busAdapter;
    private readonly Notifier _notifier;

    public CreateShipmentCommandHandler(BusAdapter busAdapter, Notifier notifier)
    {
        _busAdapter = busAdapter;
        _notifier = notifier;
    }

    public async Task<AggregateResult<IShipment, IShipmentAnemicModel>> Handle(
        CreateShipmentCommand request,
        CancellationToken cancellationToken)
    {
        // Validate that items are provided.
        var hasItemsValidator = new HasItemsValidator();

        // Build anemic model from command properties.
        var model = new AnemicModel
        {
            Root = ShipmentRoot.CreateInstance(
                id: Guid.NewGuid(),
                orderId: request.OrderId,
                trackingNumber: string.Empty,
                carrier: request.Carrier,
                shippingAddress: request.ShippingAddress,
                status: "Pending",
                createdAt: DateTime.UtcNow),
            Items = request.Items
                .Select(i => ShipmentItem.CreateInstance(i.VariantId, i.ProductName, i.Quantity))
                .ToList<IShipmentItem>()
        };

        // Build aggregate result via domain logic.
        var result = AggregateResult<IShipment, IShipmentAnemicModel>.Create(model, "CreateShipment");

        await _busAdapter.PublishAsync(result, cancellationToken);
        _notifier.Notify(result);

        return result;
    }
}
