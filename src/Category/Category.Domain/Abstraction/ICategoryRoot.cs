namespace Category.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface ICategoryRoot : IValueObject
{
    string Name { get; }
    Guid? ParentId { get; }
    int Depth { get; }
    int SortOrder { get; }
    string Status { get; }
}
