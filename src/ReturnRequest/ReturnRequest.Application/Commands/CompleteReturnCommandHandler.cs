using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;
using ReturnRequest.Domain.Implementation;
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

        var model = new AnemicModel
        {
            Root = ReturnRequestRoot.CreateInstance(
                id: request.ReturnRequestId,
                orderId: Guid.Empty,
                customerId: Guid.Empty,
                rmaNumber: string.Empty,
                reason: string.Empty,
                status: "Completed",
                requestedAt: DateTime.UtcNow),
            RefundAmount = request.RefundAmount
        };

        var result = AggregateResult<IReturnRequest, IReturnRequestAnemicModel>.Create(model, "CompleteReturn");

        await _busAdapter.PublishAsync(result, cancellationToken);
        _notifier.Notify(result);

        return result;
    }
}
