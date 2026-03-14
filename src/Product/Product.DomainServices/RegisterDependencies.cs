namespace Product.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Events;
using Product.Domain;
using Product.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .As<IAggregateProvider<IProduct, IProductAnemicModel>>()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<IProduct>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<IProduct>>()
            .SingleInstance();
    }
}
