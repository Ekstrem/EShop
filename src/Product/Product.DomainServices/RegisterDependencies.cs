namespace Product.DomainServices;

using Autofac;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using Product.Domain;
using Product.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<IProduct>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<IProduct>>()
            .SingleInstance();
    }
}
