using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Specifications;
using Shipment.DomainServices;

namespace Shipment.Application.Commands;

/// <summary>
/// Handles the DispatchShipmentCommand.
/// </summary>
public sealed class DispatchShipmentCommandHandler
    : IRequestHandler<DispatchShipmentCommand, AggregateResult<IShipment, IShipmentAnemicModel>>
{
    private readonly AggregateProvider _aggregateProvider;
    private readonly BusAdapter _busAdapter;
    private readonly Notifier _notifier;

    public DispatchShipmentCommandHandler(
        AggregateProvider aggregateProvider,
        BusAdapter busAdapter,
        Notifier notifier)
    {
        _aggregateProvider = aggregateProvider;
        _busAdapter = busAdapter;
        _notifier = notifier;
    }

    public async Task<AggregateResult<IShipment, IShipmentAnemicModel>> Handle(
        DispatchShipmentCommand request,
        CancellationToken cancellationToken)
    {
        var aggregate = await _aggregateProvider.GetByIdAsync(request.ShipmentId, cancellationToken);

        var isPackedValidator = new IsPackedValidator();
        var hasTrackingValidator = new HasTrackingNumberValidator();
        var hasLabelValidator = new HasLabelValidator();

        var result = AggregateResult<IShipment, IShipmentAnemicModel>.CreateInstance(
            "DispatchShipment",
            $"Shipment {request.ShipmentId} dispatched with tracking {request.TrackingNumber}.");

        await _busAdapter.PublishAsync(result, cancellationToken);
        await _notifier.NotifyAsync(result, cancellationToken);

        return result;
    }
}
