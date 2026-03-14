using ReturnRequest.Storage.Entities;

namespace ReturnRequest.Storage;

/// <summary>
/// Command repository for persisting ReturnRequest aggregates.
/// </summary>
public sealed class Repository
{
    private readonly CommandDbContext _dbContext;

    public Repository(CommandDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReturnRequestEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ReturnRequests.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task AddAsync(ReturnRequestEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.ReturnRequests.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ReturnRequestEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.ReturnRequests.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
