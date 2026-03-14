namespace DiscountCode.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Events;
using DiscountCode.Domain;
using DiscountCode.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .As<IAggregateProvider<IDiscountCode, IDiscountCodeAnemicModel>>()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<IDiscountCode>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<IDiscountCode>>()
            .SingleInstance();
    }
}
