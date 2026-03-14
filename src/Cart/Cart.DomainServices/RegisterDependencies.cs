namespace Cart.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain;
using Cart.Domain.Abstraction;

public class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<BusAdapter>()
            .As<IObserver<AggregateResult<ICart, ICartAnemicModel>>>()
            .InstancePerLifetimeScope();

        builder.RegisterDecorator<Notifier, IObserver<AggregateResult<ICart, ICartAnemicModel>>>();
    }
}
