namespace Notification.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Events;
using Notification.Domain;
using Notification.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .As<IAggregateProvider<INotification, INotificationAnemicModel>>()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<INotification>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<INotification>>()
            .SingleInstance();
    }
}
