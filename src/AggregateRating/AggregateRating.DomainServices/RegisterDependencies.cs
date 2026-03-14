namespace AggregateRating.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Events;
using AggregateRating.Domain;
using AggregateRating.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .As<IAggregateProvider<IAggregateRating, IAggregateRatingAnemicModel>>()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<IAggregateRating>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<IAggregateRating>>()
            .SingleInstance();
    }
}
