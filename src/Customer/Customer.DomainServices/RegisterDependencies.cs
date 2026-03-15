using Autofac;
using Customer.Domain;
using Customer.Domain.Abstraction;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;

namespace Customer.DomainServices;

/// <summary>
/// Autofac module that registers all Customer domain service dependencies.
/// </summary>
public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<BusAdapter>()
            .As<IObserver<AggregateResult<ICustomer, ICustomerAnemicModel>>>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Notifier>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}
