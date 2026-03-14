namespace Product.Domain.Implementation;

using Product.Domain.Abstraction;

internal sealed class ProductRoot : IProductRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid CategoryId { get; private set; }
    public string Status { get; private set; } = "Draft";

    private ProductRoot() { }

    public static IProductRoot CreateInstance(
        string name,
        string description,
        Guid categoryId,
        string status = "Draft")
        => new ProductRoot
        {
            Name = name,
            Description = description,
            CategoryId = categoryId,
            Status = status
        };
}
