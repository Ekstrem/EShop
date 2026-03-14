using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Specifications;
using Shipment.DomainServices;

namespace Shipment.Application.Commands;

/// <summary>
/// Handles the MarkAsPackedCommand.
/// </summary>
public sealed class MarkAsPackedCommandHandler
    : IRequestHandler<MarkAsPackedCommand, AggregateResult<IShipment, IShipmentAnemicModel>>
{
    private readonly AggregateProvider _aggregateProvider;
    private readonly BusAdapter _busAdapter;
    private readonly Notifier _notifier;

    public MarkAsPackedCommandHandler(
        AggregateProvider aggregateProvider,
        BusAdapter busAdapter,
        Notifier notifier)
    {
        _aggregateProvider = aggregateProvider;
        _busAdapter = busAdapter;
        _notifier = notifier;
    }

    public async Task<AggregateResult<IShipment, IShipmentAnemicModel>> Handle(
        MarkAsPackedCommand request,
        CancellationToken cancellationToken)
    {
        var aggregate = await _aggregateProvider.GetByIdAsync(request.ShipmentId, cancellationToken);

        var isPendingValidator = new IsPendingValidator();
        var hasItemsValidator = new HasItemsValidator();

        var result = AggregateResult<IShipment, IShipmentAnemicModel>.CreateInstance(
            "MarkAsPacked",
            $"Shipment {request.ShipmentId} marked as packed.");

        await _busAdapter.PublishAsync(result, cancellationToken);
        await _notifier.NotifyAsync(result, cancellationToken);

        return result;
    }
}
