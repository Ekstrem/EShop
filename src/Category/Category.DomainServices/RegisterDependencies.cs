namespace Category.DomainServices;

using Autofac;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using Category.Domain;
using Category.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<ICategory>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<ICategory>>()
            .SingleInstance();
    }
}
