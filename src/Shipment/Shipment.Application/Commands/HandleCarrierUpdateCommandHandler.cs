using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Implementation;
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

        var model = new AnemicModel
        {
            Root = ShipmentRoot.CreateInstance(
                id: request.ShipmentId,
                orderId: Guid.Empty,
                trackingNumber: string.Empty,
                carrier: string.Empty,
                shippingAddress: string.Empty,
                status: request.NewStatus,
                createdAt: DateTime.UtcNow)
        };

        var result = AggregateResultExtensions.CreateResult<IShipment, IShipmentAnemicModel>(model, "HandleCarrierUpdate");

        await _busAdapter.PublishAsync(result, cancellationToken);
        _notifier.Notify(result);

        return result;
    }
}
