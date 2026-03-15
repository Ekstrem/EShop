namespace Invoice.Application.Commands;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using MediatR;
using Invoice.Domain;
using Invoice.Domain.Abstraction;

public sealed class ResendInvoiceCommand : IRequest<AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    public Guid InvoiceId { get; init; }
}
