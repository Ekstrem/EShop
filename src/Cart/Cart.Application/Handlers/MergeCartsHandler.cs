namespace Cart.Application.Handlers;

using Hive.SeedWorks.TacticalPatterns;
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
            throw new InvalidOperationException("Target cart: " + activeValidator.ErrorMessage);

        if (!activeValidator.IsSatisfiedBy(source))
            throw new InvalidOperationException("Source cart: " + activeValidator.ErrorMessage);

        var result = CartAggregate.MergeCarts(target, source);

        await _provider.SaveAsync(result, ct);
        _observer.OnNext(result);

        return result;
    }
}
