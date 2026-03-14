namespace StockItem.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Events;
using StockItem.Domain;
using StockItem.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .As<IAggregateProvider<IStockItem, IStockItemAnemicModel>>()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<IStockItem>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<IStockItem>>()
            .SingleInstance();
    }
}
