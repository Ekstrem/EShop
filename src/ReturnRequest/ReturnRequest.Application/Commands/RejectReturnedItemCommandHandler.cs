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
/// Handles the RejectReturnedItemCommand.
/// </summary>
public sealed class RejectReturnedItemCommandHandler
    : IRequestHandler<RejectReturnedItemCommand, AggregateResult<IReturnRequest, IReturnRequestAnemicModel>>
{
    private readonly AggregateProvider _aggregateProvider;
    private readonly BusAdapter _busAdapter;
    private readonly Notifier _notifier;

    public RejectReturnedItemCommandHandler(
        AggregateProvider aggregateProvider,
        BusAdapter busAdapter,
        Notifier notifier)
    {
        _aggregateProvider = aggregateProvider;
        _busAdapter = busAdapter;
        _notifier = notifier;
    }

    public async Task<AggregateResult<IReturnRequest, IReturnRequestAnemicModel>> Handle(
        RejectReturnedItemCommand request,
        CancellationToken cancellationToken)
    {
        var aggregate = await _aggregateProvider.GetByIdAsync(request.ReturnRequestId, cancellationToken);

        var isReceivedValidator = new IsReceivedValidator();

        var model = new AnemicModel
        {
            Root = ReturnRequestRoot.CreateInstance(
                id: request.ReturnRequestId,
                orderId: Guid.Empty,
                customerId: Guid.Empty,
                rmaNumber: string.Empty,
                reason: request.RejectionReason,
                status: "RejectedAfterInspection",
                requestedAt: DateTime.UtcNow)
        };

        var result = AggregateResultExtensions.CreateResult<IReturnRequest, IReturnRequestAnemicModel>(model, "RejectReturnedItem");

        await _busAdapter.PublishAsync(result, cancellationToken);
        _notifier.Notify(result);

        return result;
    }
}
