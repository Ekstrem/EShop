using Microsoft.EntityFrameworkCore;
using ReturnRequest.InternalContracts;

namespace ReturnRequest.Storage;

/// <summary>
/// Implementation of the return request query repository using the read database context.
/// </summary>
public sealed class ReturnRequestQueryRepository : IReturnRequestQueryRepository
{
    private readonly ReadDbContext _readDbContext;

    public ReturnRequestQueryRepository(ReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<ReturnRequestReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _readDbContext.ReturnRequests
            .Include(r => r.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<IReadOnlyList<ReturnRequestReadModel>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var entities = await _readDbContext.ReturnRequests
            .Include(r => r.Items)
            .AsNoTracking()
            .Where(r => r.OrderId == orderId)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    public async Task<IReadOnlyList<ReturnRequestReadModel>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var entities = await _readDbContext.ReturnRequests
            .Include(r => r.Items)
            .AsNoTracking()
            .Where(r => r.CustomerId == customerId)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    public async Task<IReadOnlyList<ReturnRequestReadModel>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        var entities = await _readDbContext.ReturnRequests
            .Include(r => r.Items)
            .AsNoTracking()
            .Where(r => r.Status == status)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    public async Task<ReturnRequestReadModel?> GetByRmaNumberAsync(string rmaNumber, CancellationToken cancellationToken = default)
    {
        var entity = await _readDbContext.ReturnRequests
            .Include(r => r.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RmaNumber == rmaNumber, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    private static ReturnRequestReadModel MapToReadModel(Entities.ReturnRequestEntity entity)
    {
        return new ReturnRequestReadModel
        {
            Id = entity.Id,
            OrderId = entity.OrderId,
            CustomerId = entity.CustomerId,
            RmaNumber = entity.RmaNumber,
            Reason = entity.Reason,
            Status = entity.Status,
            RequestedAt = entity.RequestedAt,
            RefundAmount = entity.RefundAmount,
            Items = entity.Items.Select(i => new ReturnItemReadModel
            {
                VariantId = i.VariantId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList(),
            ReturnLabel = entity.ReturnLabelUrl is not null
                ? new ReturnLabelReadModel
                {
                    LabelUrl = entity.ReturnLabelUrl,
                    Carrier = entity.ReturnLabelCarrier ?? string.Empty
                }
                : null
        };
    }
}
