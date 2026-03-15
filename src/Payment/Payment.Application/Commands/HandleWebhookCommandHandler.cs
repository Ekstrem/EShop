namespace Payment.Application.Commands;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using MediatR;
using Payment.Domain;
using Payment.Domain.Abstraction;
using Payment.DomainServices;

public sealed class HandleWebhookCommandHandler
    : IRequestHandler<HandleWebhookCommand, AggregateResult<IPayment, IPaymentAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<IPayment, IPaymentAnemicModel>> _busAdapter;

    public HandleWebhookCommandHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<IPayment, IPaymentAnemicModel>> busAdapter)
    {
        _provider = provider;
        _busAdapter = busAdapter;
    }

    public async Task<AggregateResult<IPayment, IPaymentAnemicModel>> Handle(
        HandleWebhookCommand request, CancellationToken cancellationToken)
    {
        var notifier = await _provider.LoadAsync(request.PaymentId, cancellationToken);
        notifier.Subscribe(_busAdapter);

        var result = notifier.HandleProviderWebhook(
            request.ProviderTransactionId,
            request.TransactionType,
            request.Amount,
            request.TransactionStatus);

        return result;
    }
}
