using DigiTFactory.Libraries.SeedWorks.Result;
using EShop.Contracts;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Implementation;
using Shipment.DomainServices;

namespace Shipment.Application.Commands;

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
        var model = await _aggregateProvider.GetByIdAsync(request.ShipmentId, cancellationToken);
        var aggregate = Aggregate.CreateInstance(model);

        var result = aggregate.DispatchShipment(request.TrackingNumber, request.LabelUrl);

        if (result.IsSuccess())
        {
            await _busAdapter.PublishAsync(result, cancellationToken);
            _notifier.Notify(result);
        }

        return result;
    }
}
