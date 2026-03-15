using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Microsoft.EntityFrameworkCore;
using Session.Domain;
using Session.Domain.Abstraction;
using Session.Domain.Implementation;

namespace Session.Storage;

/// <summary>
/// Repository implementation for Session aggregate persistence.
/// </summary>
public sealed class SessionRepository : IRepository<ISession, ISessionAnemicModel>
{
    private readonly CommandDbContext _dbContext;

    public SessionRepository(CommandDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ISessionAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _dbContext.Sessions
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (entity is null) return null;

        var root = SessionRoot.CreateInstance(
            entity.Id, entity.CustomerId, entity.Token,
            entity.ExpiresAt, entity.Status, entity.CreatedAt, entity.DeviceInfo);

        return SessionAnemicModel.CreateInstance(entity.Id, root);
    }

    public async Task SaveAsync(AggregateResult<ISession, ISessionAnemicModel> result, CancellationToken ct = default)
    {
        var model = result.Model!;
        var existing = await _dbContext.Sessions
            .FirstOrDefaultAsync(s => s.Id == model.Id, ct);

        if (existing is null)
        {
            var entity = new SessionEntity
            {
                Id = model.Id,
                CustomerId = model.Root.CustomerId,
                Token = model.Root.Token,
                ExpiresAt = model.Root.ExpiresAt,
                Status = model.Root.Status,
                CreatedAt = model.Root.CreatedAt,
                DeviceInfo = model.Root.DeviceInfo
            };
            _dbContext.Sessions.Add(entity);
        }
        else
        {
            existing.Token = model.Root.Token;
            existing.ExpiresAt = model.Root.ExpiresAt;
            existing.Status = model.Root.Status;
            existing.DeviceInfo = model.Root.DeviceInfo;
        }

        await _dbContext.SaveChangesAsync(ct);
    }
}
