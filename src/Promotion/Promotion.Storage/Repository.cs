namespace Promotion.Storage;

using Microsoft.EntityFrameworkCore;
using Promotion.InternalContracts;

public sealed class Repository : IPromotionQueryRepository
{
    private readonly CommandDbContext _context;

    public Repository(CommandDbContext context)
    {
        _context = context;
    }

    public async Task<PromotionReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Promotions
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<IReadOnlyList<PromotionReadModel>> SearchAsync(
        string? name,
        string? discountType,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Promotions.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(e => e.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(discountType))
            query = query.Where(e => e.DiscountType == discountType);
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.Status == status);

        var entities = await query
            .OrderByDescending(e => e.StartDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    public async Task<int> CountAsync(
        string? name,
        string? discountType,
        string? status,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Promotions.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(e => e.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(discountType))
            query = query.Where(e => e.DiscountType == discountType);
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.Status == status);

        return await query.CountAsync(cancellationToken);
    }

    private static PromotionReadModel MapToReadModel(PromotionEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        DiscountType = entity.DiscountType,
        DiscountValue = entity.DiscountValue,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        Status = entity.Status,
        Conditions = entity.Conditions,
        AllowStacking = entity.AllowStacking
    };
}
