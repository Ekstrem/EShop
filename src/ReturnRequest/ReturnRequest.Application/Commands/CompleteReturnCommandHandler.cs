using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;
using ReturnRequest.Domain.Specifications;
using ReturnRequest.DomainServices;

namespace ReturnRequest.Application.Commands;

/// <summary>
/// Handles the CompleteReturnCommand.
/// </summary>
public sealed class CompleteReturnCommandHandler
    : IRequestHandler<CompleteReturnCommand, AggregateResult<IReturnRequest, IReturnRequestAnemicModel>>
{
    private readonly AggregateProvider _aggregateProvider;
    private readonly BusAdapter _busAdapter;
    private readonly Notifier _notifier;

    public CompleteReturnCommandHandler(
        AggregateProvider aggregateProvider,
        BusAdapter busAdapter,
        Notifier notifier)
    {
        _aggregateProvider = aggregateProvider;
        _busAdapter = busAdapter;
        _notifier = notifier;
    }

    public async Task<AggregateResult<IReturnRequest, IReturnRequestAnemicModel>> Handle(
        CompleteReturnCommand request,
        CancellationToken cancellationToken)
    {
        var aggregate = await _aggregateProvider.GetByIdAsync(request.ReturnRequestId, cancellationToken);

        var isReceivedValidator = new IsReceivedValidator();
        var refundAmountValidator = new RefundAmountValidator();

        var result = AggregateResult<IReturnRequest, IReturnRequestAnemicModel>.CreateInstance(
            "CompleteReturn",
            $"Return request {request.ReturnRequestId} completed with refund {request.RefundAmount}.");

        await _busAdapter.PublishAsync(result, cancellationToken);
        await _notifier.NotifyAsync(result, cancellationToken);

        return result;
    }
}
