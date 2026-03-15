namespace Order.Application.Handlers;

using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Order.Domain;
using Order.Domain.Abstraction;
using Order.Domain.Aggregate;
using Order.Domain.Specifications;
using Order.DomainServices;
using Order.Application.Commands;
using MediatR;

public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, AggregateResult<IOrder, IOrderAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<IOrder, IOrderAnemicModel>> _observer;

    public CancelOrderHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<IOrder, IOrderAnemicModel>> observer)
    {
        _provider = provider;
        _observer = observer;
    }

    public async Task<AggregateResult<IOrder, IOrderAnemicModel>> Handle(
        CancelOrderCommand request,
        CancellationToken ct)
    {
        var model = await _provider.GetByIdAsync(request.OrderId, ct)
            ?? throw new InvalidOperationException("Order not found.");

        var validator = new CanCancelValidator();
        if (!validator.IsSatisfiedBy(model))
            throw new InvalidOperationException(validator.ErrorMessage);

        var result = OrderAggregate.CancelOrder(model);

        await _provider.SaveAsync(result, ct);
        _observer.OnNext(result);

        return result;
    }
}
