namespace Category.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface ICategoryRoot : IAggregateRoot<ICategory>
{
    string Name { get; }
    Guid? ParentId { get; }
    int Depth { get; }
    int SortOrder { get; }
    string Status { get; }
}
