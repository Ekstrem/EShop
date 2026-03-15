using Autofac;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Session.Domain;
using Session.Domain.Abstraction;

namespace Session.DomainServices;

/// <summary>
/// Autofac module that registers all Session domain service dependencies.
/// </summary>
public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<BusAdapter>()
            .As<IObserver<AggregateResult<ISession, ISessionAnemicModel>>>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Notifier>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}
