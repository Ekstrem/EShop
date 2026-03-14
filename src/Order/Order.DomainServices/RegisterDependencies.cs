namespace Order.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
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
