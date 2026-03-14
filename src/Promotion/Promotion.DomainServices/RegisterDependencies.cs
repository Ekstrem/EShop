namespace Promotion.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Events;
using Promotion.Domain;
using Promotion.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .As<IAggregateProvider<IPromotion, IPromotionAnemicModel>>()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<IPromotion>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<IPromotion>>()
            .SingleInstance();
    }
}
