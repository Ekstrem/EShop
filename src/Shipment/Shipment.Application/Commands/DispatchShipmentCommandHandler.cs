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

        var model = new AnemicModel
        {
            Root = ShipmentRoot.CreateInstance(
                id: request.ShipmentId,
                orderId: Guid.Empty,
                trackingNumber: request.TrackingNumber,
                carrier: string.Empty,
                shippingAddress: string.Empty,
                status: "Shipped",
                createdAt: DateTime.UtcNow),
            Label = ShippingLabel.CreateInstance(request.LabelUrl, DateTime.UtcNow)
        };

        var result = AggregateResultExtensions.CreateResult<IShipment, IShipmentAnemicModel>(model, "DispatchShipment");

        await _busAdapter.PublishAsync(result, cancellationToken);
        _notifier.Notify(result);

        return result;
    }
}
