using DigiTFactory.Libraries.SeedWorks.Result;
using EShop.Contracts;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;
using ReturnRequest.Domain.Implementation;
using ReturnRequest.DomainServices;

namespace ReturnRequest.Application.Commands;

/// <summary>
/// Handles the RejectReturnCommand.
/// </summary>
public sealed class RejectReturnCommandHandler
    : IRequestHandler<RejectReturnCommand, AggregateResult<IReturnRequest, IReturnRequestAnemicModel>>
{
    private readonly AggregateProvider _aggregateProvider;
    private readonly BusAdapter _busAdapter;
    private readonly Notifier _notifier;

    public RejectReturnCommandHandler(
        AggregateProvider aggregateProvider,
        BusAdapter busAdapter,
        Notifier notifier)
    {
        _aggregateProvider = aggregateProvider;
        _busAdapter = busAdapter;
        _notifier = notifier;
    }

    public async Task<AggregateResult<IReturnRequest, IReturnRequestAnemicModel>> Handle(
        RejectReturnCommand request,
        CancellationToken cancellationToken)
    {
        var model = await _aggregateProvider.GetByIdAsync(request.ReturnRequestId, cancellationToken);
        var aggregate = Aggregate.CreateInstance(model!);

        var result = aggregate.RejectReturn(request.RejectionReason);

        if (result.IsSuccess())
        {
            await _busAdapter.PublishAsync(result, cancellationToken);
            _notifier.Notify(result);
        }

        return result;
    }
}
