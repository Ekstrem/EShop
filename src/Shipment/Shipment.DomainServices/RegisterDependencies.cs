using Autofac;
using Shipment.InternalContracts;

namespace Shipment.DomainServices;

/// <summary>
/// Registers domain service dependencies for the Shipment context.
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
