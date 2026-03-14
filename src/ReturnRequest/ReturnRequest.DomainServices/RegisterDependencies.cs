using Autofac;
using ReturnRequest.InternalContracts;

namespace ReturnRequest.DomainServices;

/// <summary>
/// Registers domain service dependencies for the ReturnRequest context.
/// </summary>
public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<BusAdapter>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<Notifier>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}
