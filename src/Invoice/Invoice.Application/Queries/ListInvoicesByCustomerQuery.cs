namespace Invoice.Application.Queries;

using MediatR;
using Invoice.InternalContracts;

public sealed class ListInvoicesByCustomerQuery : IRequest<IReadOnlyList<InvoiceReadModel>>
{
    public Guid CustomerId { get; init; }
}
