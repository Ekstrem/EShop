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

public class RemoveItemFromCartHandler : IRequestHandler<RemoveItemFromCartCommand, AggregateResult<ICart, ICartAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<ICart, ICartAnemicModel>> _observer;

    public RemoveItemFromCartHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<ICart, ICartAnemicModel>> observer)
    {
        _provider = provider;
        _observer = observer;
    }

    public async Task<AggregateResult<ICart, ICartAnemicModel>> Handle(
        RemoveItemFromCartCommand request,
        CancellationToken ct)
    {
        var model = await _provider.GetByIdAsync(request.CartId, ct)
            ?? throw new InvalidOperationException("Cart not found.");

        var activeValidator = new IsActiveCartValidator();
        if (!activeValidator.IsSatisfiedBy(model))
            throw new InvalidOperationException(activeValidator.ErrorMessage);

        if (model.Items.All(i => i.VariantId != request.VariantId))
            throw new InvalidOperationException("Item not found in cart.");

        var result = CartAggregate.RemoveItemFromCart(model, request.VariantId);

        await _provider.SaveAsync(result, ct);
        _observer.OnNext(result);

        return result;
    }
}
