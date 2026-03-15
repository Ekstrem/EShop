using DigiTFactory.Libraries.SeedWorks.Result;
using EShop.Contracts;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;
using ReturnRequest.Domain.Implementation;
using ReturnRequest.DomainServices;

namespace ReturnRequest.Application.Commands;

/// <summary>
/// Handles the RequestReturnCommand.
/// </summary>
public sealed class RequestReturnCommandHandler
    : IRequestHandler<RequestReturnCommand, AggregateResult<IReturnRequest, IReturnRequestAnemicModel>>
{
    private readonly BusAdapter _busAdapter;
    private readonly Notifier _notifier;

    public RequestReturnCommandHandler(BusAdapter busAdapter, Notifier notifier)
    {
        _busAdapter = busAdapter;
        _notifier = notifier;
    }

    public async Task<AggregateResult<IReturnRequest, IReturnRequestAnemicModel>> Handle(
        RequestReturnCommand request,
        CancellationToken cancellationToken)
    {
        var aggregate = Aggregate.CreateInstance(new AnemicModel());

        var items = request.Items
            .Select(i => ReturnItem.CreateInstance(i.VariantId, i.ProductName, i.Quantity, i.UnitPrice))
            .ToList<IReturnItem>();

        var result = aggregate.RequestReturn(
            request.OrderId, request.CustomerId, request.Reason, items, request.OrderDeliveredAt);

        if (result.IsSuccess())
        {
            await _busAdapter.PublishAsync(result, cancellationToken);
            _notifier.Notify(result);
        }

        return result;
    }
}
