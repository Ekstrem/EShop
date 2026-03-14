namespace Product.Storage;

using Microsoft.EntityFrameworkCore;
using Product.InternalContracts;

public sealed class ProductRepository : IProductQueryRepository
{
    private readonly ProductReadDbContext _context;

    public ProductRepository(ProductReadDbContext context)
    {
        _context = context;
    }

    public async Task<ProductReadModel?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Products
            .Include(p => p.Variants)
            .Include(p => p.Media)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<IReadOnlyList<ProductReadModel>> SearchAsync(
        string? name,
        Guid? categoryId,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Products
            .Include(p => p.Variants)
            .Include(p => p.Media)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(p => p.Status == status);

        var entities = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToReadModel).ToList();
    }

    public async Task<int> CountAsync(
        string? name,
        Guid? categoryId,
        string? status,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(p => p.Status == status);

        return await query.CountAsync(cancellationToken);
    }

    private static ProductReadModel MapToReadModel(ProductEntity entity)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            CategoryId = entity.CategoryId,
            Status = entity.Status,
            Variants = entity.Variants.Select(v => new ProductVariantReadModel
            {
                Sku = v.Sku,
                Size = v.Size,
                Color = v.Color,
                Price = v.Price
            }).ToList(),
            Media = entity.Media.Select(m => new ProductMediaReadModel
            {
                Url = m.Url,
                Alt = m.Alt,
                SortOrder = m.SortOrder
            }).ToList()
        };
}
