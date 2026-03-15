using DigiTFactory.Libraries.SeedWorks.Result;
using EShop.Contracts;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;
using ReturnRequest.Domain.Implementation;
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
        var model = await _aggregateProvider.GetByIdAsync(request.ReturnRequestId, cancellationToken);
        var aggregate = Aggregate.CreateInstance(model!);

        var result = aggregate.ConfirmReturnShipped();

        if (result.IsSuccess())
        {
            await _busAdapter.PublishAsync(result, cancellationToken);
            _notifier.Notify(result);
        }

        return result;
    }
}
