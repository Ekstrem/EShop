namespace Category.Domain.Implementation;

using Category.Domain.Abstraction;

internal sealed class CategoryRoot : ICategoryRoot
{
    public string Name { get; private set; } = string.Empty;
    public Guid? ParentId { get; private set; }
    public int Depth { get; private set; }
    public int SortOrder { get; private set; }
    public string Status { get; private set; } = "Active";

    private CategoryRoot() { }

    public static ICategoryRoot CreateInstance(
        string name,
        Guid? parentId,
        int depth,
        int sortOrder,
        string status = "Active")
        => new CategoryRoot
        {
            Name = name,
            ParentId = parentId,
            Depth = depth,
            SortOrder = sortOrder,
            Status = status
        };
}
