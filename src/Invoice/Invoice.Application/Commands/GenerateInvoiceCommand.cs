namespace Invoice.Application.Commands;

using Hive.SeedWorks.Result;
using MediatR;
using Invoice.Domain;
using Invoice.Domain.Abstraction;

public sealed class GenerateInvoiceCommand : IRequest<AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    public string InvoiceNumber { get; init; } = string.Empty;
    public Guid OrderId { get; init; }
    public Guid CustomerId { get; init; }
    public IReadOnlyList<InvoiceLineDto> Lines { get; init; } = new List<InvoiceLineDto>();
}

public sealed class InvoiceLineDto
{
    public string Description { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal VatRate { get; init; }
}
