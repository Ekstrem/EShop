namespace Review.DomainServices;

using Autofac;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Events;
using Review.Domain;
using Review.Domain.Abstraction;

public sealed class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AggregateProvider>()
            .As<IAggregateProvider<IReview, IReviewAnemicModel>>()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .As<IBusAdapter<IReview>>()
            .SingleInstance();

        builder.RegisterType<Notifier>()
            .As<INotifier<IReview>>()
            .SingleInstance();
    }
}
