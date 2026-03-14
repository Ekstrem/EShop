namespace Invoice.Application.Commands;

using Hive.SeedWorks.Result;
using MediatR;
using Invoice.Domain;
using Invoice.Domain.Abstraction;

public sealed class GenerateCreditNoteCommand : IRequest<AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    public Guid OriginalInvoiceId { get; init; }
    public string CreditNoteNumber { get; init; } = string.Empty;
    public decimal RefundAmount { get; init; }
}
