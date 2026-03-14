namespace Product.InternalContracts;

public sealed class ProductReadModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid CategoryId { get; init; }
    public string Status { get; init; } = string.Empty;
    public IReadOnlyList<ProductVariantReadModel> Variants { get; init; } = new List<ProductVariantReadModel>();
    public IReadOnlyList<ProductMediaReadModel> Media { get; init; } = new List<ProductMediaReadModel>();
}

public sealed class ProductVariantReadModel
{
    public string Sku { get; init; } = string.Empty;
    public string Size { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public decimal Price { get; init; }
}

public sealed class ProductMediaReadModel
{
    public string Url { get; init; } = string.Empty;
    public string Alt { get; init; } = string.Empty;
    public int SortOrder { get; init; }
}
