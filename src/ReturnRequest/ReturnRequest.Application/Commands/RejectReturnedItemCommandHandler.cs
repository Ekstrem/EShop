using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;
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

        var result = AggregateResult<IReturnRequest, IReturnRequestAnemicModel>.CreateInstance(
            "RejectReturnedItem",
            $"Return request {request.ReturnRequestId} rejected after inspection: {request.RejectionReason}.");

        await _busAdapter.PublishAsync(result, cancellationToken);
        await _notifier.NotifyAsync(result, cancellationToken);

        return result;
    }
}
