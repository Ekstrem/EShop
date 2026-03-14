namespace DiscountCode.Storage;

using Microsoft.EntityFrameworkCore;
using DiscountCode.InternalContracts;

public sealed class Repository : IDiscountCodeQueryRepository
{
    private readonly CommandDbContext _context;

    public Repository(CommandDbContext context)
    {
        _context = context;
    }

    public async Task<DiscountCodeReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.DiscountCodes
            .AsNoTracking()
            .Include(e => e.Redemptions)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<DiscountCodeReadModel?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var entity = await _context.DiscountCodes
            .AsNoTracking()
            .Include(e => e.Redemptions)
            .FirstOrDefaultAsync(e => e.Code == code, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<IReadOnlyList<DiscountCodeReadModel>> SearchAsync(
        string? code,
        Guid? promotionId,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _context.DiscountCodes
            .AsNoTracking()
            .Include(e => e.Redemptions)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(code))
            query = query.Where(e => e.Code.Contains(code));
        if (promotionId.HasValue)
            query = query.Where(e => e.PromotionId == promotionId.Value);
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
        string? code,
        Guid? promotionId,
        string? status,
        CancellationToken cancellationToken = default)
    {
        var query = _context.DiscountCodes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(code))
            query = query.Where(e => e.Code.Contains(code));
        if (promotionId.HasValue)
            query = query.Where(e => e.PromotionId == promotionId.Value);
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.Status == status);

        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.DiscountCodes
            .AnyAsync(e => e.Code == code, cancellationToken);
    }

    private static DiscountCodeReadModel MapToReadModel(DiscountCodeEntity entity) => new()
    {
        Id = entity.Id,
        Code = entity.Code,
        PromotionId = entity.PromotionId,
        MaxUsage = entity.MaxUsage,
        UsageCount = entity.UsageCount,
        Status = entity.Status,
        CreatedAt = entity.CreatedAt,
        ExpiresAt = entity.ExpiresAt,
        Redemptions = entity.Redemptions.Select(r => new RedemptionReadModel
        {
            OrderId = r.OrderId,
            RedeemedAt = r.RedeemedAt
        }).ToList()
    };
}
