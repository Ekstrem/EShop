namespace Cart.Application.Handlers;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Cart.Domain;
using Cart.Domain.Abstraction;
using Cart.Domain.Aggregate;
using Cart.Domain.Specifications;
using Cart.DomainServices;
using Cart.Application.Commands;
using MediatR;

public class MergeCartsHandler : IRequestHandler<MergeCartsCommand, AggregateResult<ICart, ICartAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<ICart, ICartAnemicModel>> _observer;

    public MergeCartsHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<ICart, ICartAnemicModel>> observer)
    {
        _provider = provider;
        _observer = observer;
    }

    public async Task<AggregateResult<ICart, ICartAnemicModel>> Handle(
        MergeCartsCommand request,
        CancellationToken ct)
    {
        var target = await _provider.GetByIdAsync(request.TargetCartId, ct)
            ?? throw new InvalidOperationException("Target cart not found.");

        var source = await _provider.GetByIdAsync(request.SourceCartId, ct)
            ?? throw new InvalidOperationException("Source cart not found.");

        var activeValidator = new IsActiveCartValidator();
        if (!activeValidator.IsSatisfiedBy(target))
            throw new InvalidOperationException("Target cart: " + activeValidator.Reason);

        if (!activeValidator.IsSatisfiedBy(source))
            throw new InvalidOperationException("Source cart: " + activeValidator.Reason);

        var result = CartAggregate.MergeCarts(target, source);

        await _provider.SaveAsync(result, ct);
        _observer.OnNext(result);

        return result;
    }
}
