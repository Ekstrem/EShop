namespace Invoice.Application.Commands;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using MediatR;
using Invoice.Domain;
using Invoice.Domain.Abstraction;

public sealed class SendInvoiceCommand : IRequest<AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    public Guid InvoiceId { get; init; }
}
