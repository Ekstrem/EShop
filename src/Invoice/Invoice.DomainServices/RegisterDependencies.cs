namespace Invoice.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.Events;
using Invoice.Domain;
using Invoice.Domain.Abstraction;
using Invoice.InternalContracts;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<BusAdapter>()
            .As<IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>>>()
            .InstancePerLifetimeScope();
    }
}
