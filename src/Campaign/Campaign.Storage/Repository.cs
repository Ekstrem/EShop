namespace Campaign.Storage;

using Microsoft.EntityFrameworkCore;
using Campaign.InternalContracts;

public sealed class Repository : ICampaignQueryRepository
{
    private readonly CommandDbContext _context;

    public Repository(CommandDbContext context)
    {
        _context = context;
    }

    public async Task<CampaignReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Campaigns
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<IReadOnlyList<CampaignReadModel>> SearchAsync(
        string? name,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Campaigns.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(e => e.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.Status == status);

        var entities = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    public async Task<int> CountAsync(
        string? name,
        string? status,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Campaigns.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(e => e.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.Status == status);

        return await query.CountAsync(cancellationToken);
    }

    private static CampaignReadModel MapToReadModel(CampaignEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Subject = entity.Subject,
        TemplateId = entity.TemplateId,
        SegmentId = entity.SegmentId,
        ScheduledAt = entity.ScheduledAt,
        Status = entity.Status,
        TotalRecipients = entity.TotalRecipients,
        SentCount = entity.SentCount,
        FailedCount = entity.FailedCount,
        CreatedAt = entity.CreatedAt
    };
}
