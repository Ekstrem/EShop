namespace Invoice.Application.Commands;

using Hive.SeedWorks.Result;
using MediatR;
using Invoice.Domain;
using Invoice.Domain.Abstraction;

public sealed class ResendInvoiceCommand : IRequest<AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    public Guid InvoiceId { get; init; }
}
