using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Specifications;
using Shipment.DomainServices;

namespace Shipment.Application.Commands;

/// <summary>
/// Handles the HandleCarrierUpdateCommand.
/// </summary>
public sealed class HandleCarrierUpdateCommandHandler
    : IRequestHandler<HandleCarrierUpdateCommand, AggregateResult<IShipment, IShipmentAnemicModel>>
{
    private readonly AggregateProvider _aggregateProvider;
    private readonly BusAdapter _busAdapter;
    private readonly Notifier _notifier;

    public HandleCarrierUpdateCommandHandler(
        AggregateProvider aggregateProvider,
        BusAdapter busAdapter,
        Notifier notifier)
    {
        _aggregateProvider = aggregateProvider;
        _busAdapter = busAdapter;
        _notifier = notifier;
    }

    public async Task<AggregateResult<IShipment, IShipmentAnemicModel>> Handle(
        HandleCarrierUpdateCommand request,
        CancellationToken cancellationToken)
    {
        var aggregate = await _aggregateProvider.GetByIdAsync(request.ShipmentId, cancellationToken);

        var isNotDeliveredValidator = new IsNotDeliveredValidator();
        var sequentialValidator = new SequentialStatusValidator(request.NewStatus);

        var result = AggregateResult<IShipment, IShipmentAnemicModel>.CreateInstance(
            "HandleCarrierUpdate",
            $"Shipment {request.ShipmentId} updated to {request.NewStatus}.");

        await _busAdapter.PublishAsync(result, cancellationToken);
        await _notifier.NotifyAsync(result, cancellationToken);

        return result;
    }
}
