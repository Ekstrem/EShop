namespace Category.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface ICategoryAnemicModel : IAnemicModel<ICategory>
{
    ICategoryRoot Root { get; }
}
