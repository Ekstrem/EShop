namespace Campaign.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Events;
using Campaign.Domain;
using Campaign.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .As<IAggregateProvider<ICampaign, ICampaignAnemicModel>>()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<ICampaign>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<ICampaign>>()
            .SingleInstance();
    }
}
