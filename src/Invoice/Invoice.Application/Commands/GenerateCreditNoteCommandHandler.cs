namespace Invoice.Application.Commands;

using Hive.SeedWorks.Result;
using MediatR;
using Invoice.Domain;
using Invoice.Domain.Abstraction;
using Invoice.DomainServices;

public sealed class GenerateCreditNoteCommandHandler
    : IRequestHandler<GenerateCreditNoteCommand, AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>> _busAdapter;

    public GenerateCreditNoteCommandHandler(
        AggregateProvider provider,
        IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>> busAdapter)
    {
        _provider = provider;
        _busAdapter = busAdapter;
    }

    public async Task<AggregateResult<IInvoice, IInvoiceAnemicModel>> Handle(
        GenerateCreditNoteCommand request, CancellationToken cancellationToken)
    {
        var notifier = await _provider.LoadAsync(request.OriginalInvoiceId, cancellationToken);
        notifier.Subscribe(_busAdapter);

        var result = notifier.GenerateCreditNote(request.CreditNoteNumber, request.RefundAmount);
        return result;
    }
}
