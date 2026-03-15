namespace Order.DomainServices;

using Autofac;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Order.Domain;
using Order.Domain.Abstraction;

public class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<BusAdapter>()
            .As<IObserver<AggregateResult<IOrder, IOrderAnemicModel>>>()
            .InstancePerLifetimeScope();

        builder.RegisterDecorator<Notifier, IObserver<AggregateResult<IOrder, IOrderAnemicModel>>>();
    }
}
