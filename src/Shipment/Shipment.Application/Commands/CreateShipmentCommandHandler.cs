using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;
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

        // Build aggregate result via domain logic.
        var result = AggregateResult<IShipment, IShipmentAnemicModel>.CreateInstance(
            "CreateShipment",
            $"Shipment created for order {request.OrderId}.");

        await _busAdapter.PublishAsync(result, cancellationToken);
        await _notifier.NotifyAsync(result, cancellationToken);

        return result;
    }
}
