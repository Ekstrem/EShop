namespace Invoice.Application.Commands;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using MediatR;
using Invoice.Domain;
using Invoice.Domain.Abstraction;
using Invoice.Domain.Implementation;
using Invoice.DomainServices;

public sealed class GenerateInvoiceCommandHandler
    : IRequestHandler<GenerateInvoiceCommand, AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>> _busAdapter;

    public GenerateInvoiceCommandHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>> busAdapter)
    {
        _provider = provider;
        _busAdapter = busAdapter;
    }

    public Task<AggregateResult<IInvoice, IInvoiceAnemicModel>> Handle(
        GenerateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var notifier = _provider.CreateNew();
        notifier.Subscribe(_busAdapter);

        var lines = request.Lines.Select(l =>
            InvoiceLine.CreateInstance(l.Description, l.Quantity, l.UnitPrice, l.VatRate))
            .ToList();

        var result = notifier.GenerateInvoice(
            request.InvoiceNumber,
            request.OrderId,
            request.CustomerId,
            lines);

        return Task.FromResult(result);
    }
}
