using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;
using ReturnRequest.Domain.Specifications;
using ReturnRequest.DomainServices;

namespace ReturnRequest.Application.Commands;

/// <summary>
/// Handles the ConfirmReturnShippedCommand.
/// </summary>
public sealed class ConfirmReturnShippedCommandHandler
    : IRequestHandler<ConfirmReturnShippedCommand, AggregateResult<IReturnRequest, IReturnRequestAnemicModel>>
{
    private readonly AggregateProvider _aggregateProvider;
    private readonly BusAdapter _busAdapter;
    private readonly Notifier _notifier;

    public ConfirmReturnShippedCommandHandler(
        AggregateProvider aggregateProvider,
        BusAdapter busAdapter,
        Notifier notifier)
    {
        _aggregateProvider = aggregateProvider;
        _busAdapter = busAdapter;
        _notifier = notifier;
    }

    public async Task<AggregateResult<IReturnRequest, IReturnRequestAnemicModel>> Handle(
        ConfirmReturnShippedCommand request,
        CancellationToken cancellationToken)
    {
        var aggregate = await _aggregateProvider.GetByIdAsync(request.ReturnRequestId, cancellationToken);

        var isApprovedValidator = new IsApprovedValidator();

        var result = AggregateResult<IReturnRequest, IReturnRequestAnemicModel>.CreateInstance(
            "ConfirmReturnShipped",
            $"Return request {request.ReturnRequestId} confirmed as shipped.");

        await _busAdapter.PublishAsync(result, cancellationToken);
        await _notifier.NotifyAsync(result, cancellationToken);

        return result;
    }
}
