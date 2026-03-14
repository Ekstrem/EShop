namespace Payment.Application.Commands;

using Hive.SeedWorks.Result;
using MediatR;
using Payment.Domain;
using Payment.Domain.Abstraction;
using Payment.DomainServices;

public sealed class RequestRefundCommandHandler
    : IRequestHandler<RequestRefundCommand, AggregateResult<IPayment, IPaymentAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<IPayment, IPaymentAnemicModel>> _busAdapter;

    public RequestRefundCommandHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<IPayment, IPaymentAnemicModel>> busAdapter)
    {
        _provider = provider;
        _busAdapter = busAdapter;
    }

    public async Task<AggregateResult<IPayment, IPaymentAnemicModel>> Handle(
        RequestRefundCommand request, CancellationToken cancellationToken)
    {
        var notifier = await _provider.LoadAsync(request.PaymentId, cancellationToken);
        notifier.Subscribe(_busAdapter);

        var result = notifier.RequestRefund(request.RefundAmount);
        return result;
    }
}
