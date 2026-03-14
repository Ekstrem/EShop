namespace Invoice.Application.Queries;

using MediatR;
using Invoice.Domain;
using Invoice.Domain.Abstraction;
using Invoice.InternalContracts;

public sealed class GetInvoiceByIdQueryHandler
    : IRequestHandler<GetInvoiceByIdQuery, InvoiceReadModel?>
{
    private readonly IQueryRepository<IInvoice, IInvoiceAnemicModel> _repository;

    public GetInvoiceByIdQueryHandler(
        IQueryRepository<IInvoice, IInvoiceAnemicModel> repository)
    {
        _repository = repository;
    }

    public async Task<InvoiceReadModel?> Handle(
        GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var model = await _repository.GetByIdAsync(request.InvoiceId, cancellationToken);
        if (model is null)
            return null;

        return new InvoiceReadModel
        {
            Id = request.InvoiceId,
            InvoiceNumber = model.Root.InvoiceNumber,
            OrderId = model.Root.OrderId,
            CustomerId = model.Root.CustomerId,
            IssueDate = model.Root.IssueDate,
            Status = model.Root.Status,
            InvoiceType = model.Root.InvoiceType,
            SubTotal = model.Totals.SubTotal,
            VatTotal = model.Totals.VatTotal,
            GrandTotal = model.Totals.GrandTotal,
            Lines = model.Lines.Select(l => new InvoiceLineReadModel
            {
                Description = l.Description,
                Quantity = l.Quantity,
                UnitPrice = l.UnitPrice,
                VatRate = l.VatRate,
                LineTotal = l.LineTotal
            }).ToList()
        };
    }
}
