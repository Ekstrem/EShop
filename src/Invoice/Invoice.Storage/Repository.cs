namespace Invoice.Storage;

using Microsoft.EntityFrameworkCore;
using Invoice.Domain;
using Invoice.Domain.Abstraction;
using Invoice.Domain.Implementation;
using Invoice.InternalContracts;

public sealed class Repository : IQueryRepository<IInvoice, IInvoiceAnemicModel>
{
    private readonly ReadDbContext _readContext;

    public Repository(ReadDbContext readContext)
    {
        _readContext = readContext;
    }

    public async Task<IInvoiceAnemicModel> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _readContext.Invoices
            .Include(i => i.Lines)
            .FirstOrDefaultAsync(i => i.Id == id, ct);

        if (entity is null)
            return new AnemicModel();

        return MapToAnemicModel(entity);
    }

    public async Task<IInvoiceAnemicModel?> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default)
    {
        var entity = await _readContext.Invoices
            .Include(i => i.Lines)
            .FirstOrDefaultAsync(i => i.OrderId == orderId, ct);

        if (entity is null)
            return null;

        return MapToAnemicModel(entity);
    }

    public async Task<IReadOnlyList<IInvoiceAnemicModel>> GetByCustomerIdAsync(
        Guid customerId, CancellationToken ct = default)
    {
        var entities = await _readContext.Invoices
            .Include(i => i.Lines)
            .Where(i => i.CustomerId == customerId)
            .ToListAsync(ct);

        return entities.Select(MapToAnemicModel).ToList();
    }

    public async Task<IReadOnlyList<IInvoiceAnemicModel>> GetAllAsync(CancellationToken ct = default)
    {
        var entities = await _readContext.Invoices
            .Include(i => i.Lines)
            .ToListAsync(ct);

        return entities.Select(MapToAnemicModel).ToList();
    }

    private static IInvoiceAnemicModel MapToAnemicModel(InvoiceEntity entity)
    {
        var root = InvoiceRoot.CreateInstance(
            entity.InvoiceNumber,
            entity.OrderId,
            entity.CustomerId,
            entity.IssueDate,
            entity.Status,
            entity.InvoiceType);

        var lines = entity.Lines.Select(l =>
            InvoiceLine.CreateInstance(
                l.Description,
                l.Quantity,
                l.UnitPrice,
                l.VatRate)).ToList();

        var totals = InvoiceTotals.CreateInstance(
            entity.SubTotal,
            entity.VatTotal,
            entity.GrandTotal);

        return new AnemicModel
        {
            Root = root,
            Lines = lines,
            Totals = totals
        };
    }
}
