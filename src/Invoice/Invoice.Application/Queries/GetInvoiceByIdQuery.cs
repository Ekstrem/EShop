namespace Invoice.Application.Queries;

using MediatR;
using Invoice.InternalContracts;

public sealed class GetInvoiceByIdQuery : IRequest<InvoiceReadModel?>
{
    public Guid InvoiceId { get; init; }
}
