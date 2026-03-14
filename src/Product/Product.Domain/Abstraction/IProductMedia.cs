namespace Product.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IProductMedia : IValueObject
{
    string Url { get; }
    string Alt { get; }
    int SortOrder { get; }
}
