using Autofac;
using Autofac.Extensions.DependencyInjection;
using AggregateRating.InternalContracts;
using AggregateRating.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AggregateRating.Application.Commands.InitializeRatingCommand).Assembly));

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<AggregateRating.DomainServices.RegisterDependencies>();
    containerBuilder.RegisterType<ReadDbContext>()
        .As<IAggregateRatingQueryRepository>()
        .InstancePerLifetimeScope();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.MapControllers();
app.Run();
