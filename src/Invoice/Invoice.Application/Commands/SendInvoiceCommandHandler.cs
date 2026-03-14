namespace Invoice.Application.Commands;

using Hive.SeedWorks.Result;
using MediatR;
using Invoice.Domain;
using Invoice.Domain.Abstraction;
using Invoice.DomainServices;

public sealed class SendInvoiceCommandHandler
    : IRequestHandler<SendInvoiceCommand, AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>> _busAdapter;

    public SendInvoiceCommandHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>> busAdapter)
    {
        _provider = provider;
        _busAdapter = busAdapter;
    }

    public async Task<AggregateResult<IInvoice, IInvoiceAnemicModel>> Handle(
        SendInvoiceCommand request, CancellationToken cancellationToken)
    {
        var notifier = await _provider.LoadAsync(request.InvoiceId, cancellationToken);
        notifier.Subscribe(_busAdapter);

        var result = notifier.SendInvoice();
        return result;
    }
}
