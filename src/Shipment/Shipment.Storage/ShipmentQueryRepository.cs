using Microsoft.EntityFrameworkCore;
using Shipment.InternalContracts;

namespace Shipment.Storage;

/// <summary>
/// Implementation of the shipment query repository using the read database context.
/// </summary>
public sealed class ShipmentQueryRepository : IShipmentQueryRepository
{
    private readonly ReadDbContext _readDbContext;

    public ShipmentQueryRepository(ReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<ShipmentReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _readDbContext.Shipments
            .Include(s => s.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<ShipmentReadModel?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var entity = await _readDbContext.Shipments
            .Include(s => s.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.OrderId == orderId, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<IReadOnlyList<ShipmentReadModel>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        var entities = await _readDbContext.Shipments
            .Include(s => s.Items)
            .AsNoTracking()
            .Where(s => s.Status == status)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    public async Task<IReadOnlyList<ShipmentReadModel>> GetByCarrierAsync(string carrier, CancellationToken cancellationToken = default)
    {
        var entities = await _readDbContext.Shipments
            .Include(s => s.Items)
            .AsNoTracking()
            .Where(s => s.Carrier == carrier)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    private static ShipmentReadModel MapToReadModel(Entities.ShipmentEntity entity)
    {
        return new ShipmentReadModel
        {
            Id = entity.Id,
            OrderId = entity.OrderId,
            TrackingNumber = entity.TrackingNumber,
            Carrier = entity.Carrier,
            ShippingAddress = entity.ShippingAddress,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            Items = entity.Items.Select(i => new ShipmentItemReadModel
            {
                VariantId = i.VariantId,
                ProductName = i.ProductName,
                Quantity = i.Quantity
            }).ToList(),
            Label = entity.LabelUrl is not null
                ? new ShippingLabelReadModel
                {
                    LabelUrl = entity.LabelUrl,
                    GeneratedAt = entity.LabelGeneratedAt ?? DateTime.MinValue
                }
                : null
        };
    }
}
