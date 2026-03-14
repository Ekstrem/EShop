using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;
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

        var result = AggregateResult<IReturnRequest, IReturnRequestAnemicModel>.CreateInstance(
            "RequestReturn",
            $"Return requested for order {request.OrderId}.");

        await _busAdapter.PublishAsync(result, cancellationToken);
        await _notifier.NotifyAsync(result, cancellationToken);

        return result;
    }
}
