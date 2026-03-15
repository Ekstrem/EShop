namespace Category.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface ICategoryAnemicModel : IAnemicModel<ICategory>
{
    ICategoryRoot Root { get; }
}
