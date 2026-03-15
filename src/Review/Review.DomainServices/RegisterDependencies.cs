namespace Review.DomainServices;

using Autofac;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<IReview>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<IReview>>()
            .SingleInstance();
    }
}
