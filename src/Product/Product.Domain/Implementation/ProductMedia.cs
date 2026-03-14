namespace Product.Domain.Implementation;

using Product.Domain.Abstraction;

internal sealed class ProductMedia : IProductMedia
{
    public string Url { get; private set; } = string.Empty;
    public string Alt { get; private set; } = string.Empty;
    public int SortOrder { get; private set; }

    private ProductMedia() { }

    public static IProductMedia CreateInstance(string url, string alt, int sortOrder)
        => new ProductMedia
        {
            Url = url,
            Alt = alt,
            SortOrder = sortOrder
        };
}
