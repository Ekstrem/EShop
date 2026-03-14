namespace Notification.Storage;

using Microsoft.EntityFrameworkCore;
using Notification.InternalContracts;

public sealed class Repository : INotificationQueryRepository
{
    private readonly CommandDbContext _context;

    public Repository(CommandDbContext context)
    {
        _context = context;
    }

    public async Task<NotificationReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Notifications
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<IReadOnlyList<NotificationReadModel>> SearchAsync(
        Guid? customerId,
        string? channel,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Notifications.AsNoTracking().AsQueryable();

        if (customerId.HasValue)
            query = query.Where(e => e.CustomerId == customerId.Value);
        if (!string.IsNullOrWhiteSpace(channel))
            query = query.Where(e => e.Channel == channel);
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
        Guid? customerId,
        string? channel,
        string? status,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Notifications.AsQueryable();

        if (customerId.HasValue)
            query = query.Where(e => e.CustomerId == customerId.Value);
        if (!string.IsNullOrWhiteSpace(channel))
            query = query.Where(e => e.Channel == channel);
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.Status == status);

        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEventAndCustomerAsync(
        Guid eventId,
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .AnyAsync(e => e.EventId == eventId && e.CustomerId == customerId, cancellationToken);
    }

    private static NotificationReadModel MapToReadModel(NotificationEntity entity) => new()
    {
        Id = entity.Id,
        CustomerId = entity.CustomerId,
        EventId = entity.EventId,
        Channel = entity.Channel,
        TemplateId = entity.TemplateId,
        Locale = entity.Locale,
        Type = entity.Type,
        Status = entity.Status,
        RetryCount = entity.RetryCount,
        RenderedContent = entity.RenderedContent,
        Subject = entity.Subject,
        CreatedAt = entity.CreatedAt
    };
}
