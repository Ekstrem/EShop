namespace Category.DomainServices;

using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Category.Domain;

public sealed class Notifier : INotifier<ICategory>
{
    public void Notify<TModel>(AggregateResult<ICategory, TModel> result)
        where TModel : IAnemicModel<ICategory>
    {
    }
}
