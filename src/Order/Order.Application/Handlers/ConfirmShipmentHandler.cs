namespace Order.Application.Handlers;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Order.Domain;
using Order.Domain.Abstraction;
using Order.Domain.Aggregate;
using Order.Domain.Specifications;
using Order.DomainServices;
using Order.Application.Commands;
using MediatR;

public class ConfirmShipmentHandler : IRequestHandler<ConfirmShipmentCommand, AggregateResult<IOrder, IOrderAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<IOrder, IOrderAnemicModel>> _observer;

    public ConfirmShipmentHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<IOrder, IOrderAnemicModel>> observer)
    {
        _provider = provider;
        _observer = observer;
    }

    public async Task<AggregateResult<IOrder, IOrderAnemicModel>> Handle(
        ConfirmShipmentCommand request,
        CancellationToken ct)
    {
        var model = await _provider.GetByIdAsync(request.OrderId, ct)
            ?? throw new InvalidOperationException("Order not found.");

        var validator = new IsPaidValidator();
        if (!validator.IsSatisfiedBy(model))
            throw new InvalidOperationException(validator.Reason);

        var result = OrderAggregate.ConfirmShipment(model);

        await _provider.SaveAsync(result, ct);
        _observer.OnNext(result);

        return result;
    }
}
