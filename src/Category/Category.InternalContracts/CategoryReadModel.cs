namespace Category.InternalContracts;

public sealed class CategoryReadModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid? ParentId { get; init; }
    public int Depth { get; init; }
    public int SortOrder { get; init; }
    public string Status { get; init; } = string.Empty;
}
