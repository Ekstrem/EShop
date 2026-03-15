namespace Cart.Application.Handlers;

using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain;
using Cart.Domain.Abstraction;
using Cart.Domain.Aggregate;
using Cart.Domain.Specifications;
using Cart.DomainServices;
using Cart.Application.Commands;
using MediatR;

public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, AggregateResult<ICart, ICartAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<ICart, ICartAnemicModel>> _observer;

    public AddItemToCartHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<ICart, ICartAnemicModel>> observer)
    {
        _provider = provider;
        _observer = observer;
    }

    public async Task<AggregateResult<ICart, ICartAnemicModel>> Handle(
        AddItemToCartCommand request,
        CancellationToken ct)
    {
        var model = await _provider.GetByIdAsync(request.CartId, ct)
            ?? throw new InvalidOperationException("Cart not found.");

        var activeValidator = new IsActiveCartValidator();
        if (!activeValidator.IsSatisfiedBy(model))
            throw new InvalidOperationException(activeValidator.ErrorMessage);

        var maxItemsValidator = new MaxItemsValidator();
        if (!maxItemsValidator.IsSatisfiedBy(model))
            throw new InvalidOperationException(maxItemsValidator.ErrorMessage);

        var quantityValidator = new QuantityRangeValidator(request.Quantity);
        if (!quantityValidator.IsSatisfiedBy(model))
            throw new InvalidOperationException(quantityValidator.ErrorMessage);

        var result = CartAggregate.AddItemToCart(
            model, request.VariantId, request.ProductName,
            request.Sku, request.Quantity, request.UnitPrice);

        await _provider.SaveAsync(result, ct);
        _observer.OnNext(result);

        return result;
    }
}
