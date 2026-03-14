namespace Payment.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.Events;
using Payment.Domain;
using Payment.Domain.Abstraction;
using Payment.InternalContracts;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<BusAdapter>()
            .As<IObserver<AggregateResult<IPayment, IPaymentAnemicModel>>>()
            .InstancePerLifetimeScope();
    }
}
