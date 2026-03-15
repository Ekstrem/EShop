namespace Product.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IProductMedia : IValueObject
{
    string Url { get; }
    string Alt { get; }
    int SortOrder { get; }
}
