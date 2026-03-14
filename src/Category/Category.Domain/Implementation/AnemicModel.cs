namespace Category.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Category.Domain.Abstraction;

internal sealed class AnemicModel : AnemicModel<ICategory>, ICategoryAnemicModel
{
    public ICategoryRoot Root { get; internal set; } = null!;
}
