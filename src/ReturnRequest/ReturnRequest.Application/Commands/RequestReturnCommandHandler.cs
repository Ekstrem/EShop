using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;
using ReturnRequest.Domain.Implementation;
using ReturnRequest.Domain.Specifications;
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
        var hasReasonValidator = new HasReasonValidator();
        var withinReturnPeriodValidator = new WithinReturnPeriodValidator(request.OrderDeliveredAt);

        var model = new AnemicModel
        {
            Root = ReturnRequestRoot.CreateInstance(
                id: Guid.NewGuid(),
                orderId: request.OrderId,
                customerId: request.CustomerId,
                rmaNumber: $"RMA-{Guid.NewGuid():N}"[..12],
                reason: request.Reason,
                status: "Requested",
                requestedAt: DateTime.UtcNow),
            Items = request.Items
                .Select(i => ReturnItem.CreateInstance(i.VariantId, i.ProductName, i.Quantity, i.UnitPrice))
                .ToList<IReturnItem>()
        };

        var result = AggregateResultExtensions.CreateResult<IReturnRequest, IReturnRequestAnemicModel>(model, "RequestReturn");

        await _busAdapter.PublishAsync(result, cancellationToken);
        _notifier.Notify(result);

        return result;
    }
}
