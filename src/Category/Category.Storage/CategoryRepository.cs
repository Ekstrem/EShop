namespace Category.Storage;

using Microsoft.EntityFrameworkCore;
using Category.InternalContracts;

public sealed class CategoryRepository : ICategoryQueryRepository
{
    private readonly CategoryReadDbContext _context;

    public CategoryRepository(CategoryReadDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryReadModel?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<IReadOnlyList<CategoryReadModel>> GetChildrenAsync(
        Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Categories
            .AsNoTracking()
            .Where(c => c.ParentId == parentId)
            .OrderBy(c => c.SortOrder)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    public async Task<IReadOnlyList<CategoryReadModel>> GetTreeAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Depth)
            .ThenBy(c => c.SortOrder)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    public async Task<IReadOnlyList<string>> GetSiblingNamesAsync(
        Guid? parentId,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Categories
            .AsNoTracking()
            .Where(c => c.ParentId == parentId);

        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);

        return await query.Select(c => c.Name).ToListAsync(cancellationToken);
    }

    public async Task<bool> HasActiveChildrenAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .AnyAsync(c => c.ParentId == categoryId && c.Status == "Active", cancellationToken);
    }

    public async Task<IReadOnlyList<Guid>> GetAncestorIdsAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        var ancestors = new List<Guid>();
        var current = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);

        while (current?.ParentId != null)
        {
            ancestors.Add(current.ParentId.Value);
            current = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == current.ParentId.Value, cancellationToken);
        }

        return ancestors;
    }

    private static CategoryReadModel MapToReadModel(CategoryEntity entity)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            ParentId = entity.ParentId,
            Depth = entity.Depth,
            SortOrder = entity.SortOrder,
            Status = entity.Status
        };
}
