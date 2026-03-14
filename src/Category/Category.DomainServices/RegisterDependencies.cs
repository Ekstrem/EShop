namespace Category.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Events;
using Category.Domain;
using Category.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .As<IAggregateProvider<ICategory, ICategoryAnemicModel>>()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<ICategory>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<ICategory>>()
            .SingleInstance();
    }
}
