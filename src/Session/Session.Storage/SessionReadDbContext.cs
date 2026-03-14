using Microsoft.EntityFrameworkCore;
using Session.InternalContracts;

namespace Session.Storage;

/// <summary>
/// Read-side DbContext for Session query projections.
/// </summary>
public sealed class SessionReadDbContext : DbContext, ISessionQueryRepository
{
    public SessionReadDbContext(DbContextOptions<SessionReadDbContext> options) : base(options)
    {
    }

    public DbSet<SessionEntity> Sessions => Set<SessionEntity>();
    public DbSet<LoginAttemptEntity> LoginAttempts => Set<LoginAttemptEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SessionEntity>(e =>
        {
            e.ToTable("Sessions");
            e.HasKey(s => s.Id);
        });

        modelBuilder.Entity<LoginAttemptEntity>(e =>
        {
            e.ToTable("LoginAttempts");
            e.HasKey(la => la.Id);
        });
    }

    public async Task<SessionReadModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await Sessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<SessionReadModel?> GetByTokenAsync(string token, CancellationToken ct = default)
    {
        var entity = await Sessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Token == token, ct);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<IReadOnlyList<SessionReadModel>> GetActiveByCustomerIdAsync(
        Guid customerId, CancellationToken ct = default)
    {
        var entities = await Sessions
            .AsNoTracking()
            .Where(s => s.CustomerId == customerId && s.Status == "Active")
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(ct);

        return entities.Select(MapToReadModel).ToList().AsReadOnly();
    }

    public async Task<int> GetActiveSessionCountAsync(
        Guid customerId, CancellationToken ct = default)
    {
        return await Sessions
            .AsNoTracking()
            .CountAsync(s => s.CustomerId == customerId && s.Status == "Active", ct);
    }

    public async Task<int> GetFailedLoginAttemptsAsync(
        Guid customerId, TimeSpan window, CancellationToken ct = default)
    {
        var windowStart = DateTime.UtcNow.Subtract(window);
        return await LoginAttempts
            .AsNoTracking()
            .CountAsync(la =>
                la.CustomerId == customerId &&
                !la.IsSuccessful &&
                la.AttemptedAt >= windowStart, ct);
    }

    public async Task<DateTime?> GetLastFailedLoginAttemptAsync(
        Guid customerId, CancellationToken ct = default)
    {
        return await LoginAttempts
            .AsNoTracking()
            .Where(la => la.CustomerId == customerId && !la.IsSuccessful)
            .OrderByDescending(la => la.AttemptedAt)
            .Select(la => (DateTime?)la.AttemptedAt)
            .FirstOrDefaultAsync(ct);
    }

    private static SessionReadModel MapToReadModel(SessionEntity entity)
    {
        return new SessionReadModel
        {
            Id = entity.Id,
            CustomerId = entity.CustomerId,
            Token = entity.Token,
            ExpiresAt = entity.ExpiresAt,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            DeviceInfo = entity.DeviceInfo
        };
    }
}
