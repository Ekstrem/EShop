namespace Invoice.Application.Queries;

using MediatR;
using Invoice.Domain;
using Invoice.Domain.Abstraction;
using Invoice.InternalContracts;

public sealed class ListInvoicesByCustomerQueryHandler
    : IRequestHandler<ListInvoicesByCustomerQuery, IReadOnlyList<InvoiceReadModel>>
{
    private readonly IQueryRepository<IInvoice, IInvoiceAnemicModel> _repository;

    public ListInvoicesByCustomerQueryHandler(
        IQueryRepository<IInvoice, IInvoiceAnemicModel> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<InvoiceReadModel>> Handle(
        ListInvoicesByCustomerQuery request, CancellationToken cancellationToken)
    {
        var models = await _repository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
        if (models is null)
            return new List<InvoiceReadModel>();

        return models.Select(model => new InvoiceReadModel
        {
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
        }).ToList();
    }
}
