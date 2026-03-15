namespace Invoice.DomainServices;

using Autofac;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
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
