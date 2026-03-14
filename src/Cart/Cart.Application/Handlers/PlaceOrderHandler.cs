namespace Cart.Application.Handlers;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain;
using Cart.Domain.Abstraction;
using Cart.Domain.Aggregate;
using Cart.Domain.Specifications;
using Cart.DomainServices;
using Cart.Application.Commands;
using MediatR;

public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, AggregateResult<ICart, ICartAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<ICart, ICartAnemicModel>> _observer;

    public PlaceOrderHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<ICart, ICartAnemicModel>> observer)
    {
        _provider = provider;
        _observer = observer;
    }

    public async Task<AggregateResult<ICart, ICartAnemicModel>> Handle(
        PlaceOrderCommand request,
        CancellationToken ct)
    {
        var model = await _provider.GetByIdAsync(request.CartId, ct)
            ?? throw new InvalidOperationException("Cart not found.");

        var activeValidator = new IsActiveCartValidator();
        if (!activeValidator.IsSatisfiedBy(model))
            throw new InvalidOperationException(activeValidator.ErrorMessage);

        var hasItemsValidator = new HasItemsForCheckoutValidator();
        if (!hasItemsValidator.IsSatisfiedBy(model))
            throw new InvalidOperationException(hasItemsValidator.ErrorMessage);

        var hasAddressValidator = new HasShippingAddressValidator();
        if (!hasAddressValidator.IsSatisfiedBy(model))
            throw new InvalidOperationException(hasAddressValidator.ErrorMessage);

        var result = CartAggregate.PlaceOrder(model);

        await _provider.SaveAsync(result, ct);
        _observer.OnNext(result);

        return result;
    }
}
