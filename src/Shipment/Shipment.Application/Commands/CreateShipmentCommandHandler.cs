using DigiTFactory.Libraries.SeedWorks.Result;
using EShop.Contracts;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Implementation;
using Shipment.DomainServices;

namespace Shipment.Application.Commands;

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
        var aggregate = Aggregate.CreateInstance(new AnemicModel());

        var items = request.Items
            .Select(i => ShipmentItem.CreateInstance(i.VariantId, i.ProductName, i.Quantity))
            .ToList<IShipmentItem>();

        var result = aggregate.CreateShipment(
            request.OrderId, request.Carrier, request.ShippingAddress, items);

        if (result.IsSuccess())
        {
            await _busAdapter.PublishAsync(result, cancellationToken);
            _notifier.Notify(result);
        }

        return result;
    }
}
