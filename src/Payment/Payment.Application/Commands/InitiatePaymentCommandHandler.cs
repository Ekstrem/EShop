namespace Payment.Application.Commands;

using Hive.SeedWorks.Result;
using MediatR;
using Payment.Domain;
using Payment.Domain.Abstraction;
using Payment.DomainServices;

public sealed class InitiatePaymentCommandHandler
    : IRequestHandler<InitiatePaymentCommand, AggregateResult<IPayment, IPaymentAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<IPayment, IPaymentAnemicModel>> _busAdapter;

    public InitiatePaymentCommandHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<IPayment, IPaymentAnemicModel>> busAdapter)
    {
        _provider = provider;
        _busAdapter = busAdapter;
    }

    public Task<AggregateResult<IPayment, IPaymentAnemicModel>> Handle(
        InitiatePaymentCommand request, CancellationToken cancellationToken)
    {
        var notifier = _provider.CreateNew();
        notifier.Subscribe(_busAdapter);

        var result = notifier.InitiatePayment(
            request.OrderId,
            request.Amount,
            request.Currency,
            request.PaymentMethod);

        return Task.FromResult(result);
    }
}
