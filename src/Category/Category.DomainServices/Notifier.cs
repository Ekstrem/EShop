namespace Category.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Category.Domain;

public sealed class Notifier : INotifier<ICategory>
{
    public void Notify<TModel>(AggregateResult<ICategory, TModel> result)
        where TModel : IAnemicModel<ICategory>
    {
    }
}
