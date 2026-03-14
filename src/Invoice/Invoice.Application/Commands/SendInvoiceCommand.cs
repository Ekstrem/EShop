namespace Invoice.Application.Commands;

using Hive.SeedWorks.Result;
using MediatR;
using Invoice.Domain;
using Invoice.Domain.Abstraction;

public sealed class SendInvoiceCommand : IRequest<AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    public Guid InvoiceId { get; init; }
}
