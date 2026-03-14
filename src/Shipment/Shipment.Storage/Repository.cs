using Shipment.Storage.Entities;

namespace Shipment.Storage;

/// <summary>
/// Command repository for persisting Shipment aggregates.
/// </summary>
public sealed class Repository
{
    private readonly CommandDbContext _dbContext;

    public Repository(CommandDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ShipmentEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Shipments.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task AddAsync(ShipmentEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Shipments.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ShipmentEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Shipments.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
