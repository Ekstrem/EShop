using Autofac;
using Autofac.Extensions.DependencyInjection;
using Review.InternalContracts;
using Review.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Review.Application.Commands.SubmitReviewCommand).Assembly));

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<Review.DomainServices.RegisterDependencies>();
    containerBuilder.RegisterType<ReadDbContext>()
        .As<IReviewQueryRepository>()
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
