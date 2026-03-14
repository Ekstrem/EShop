using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Shipment.DomainServices;
using Shipment.InternalContracts;
using Shipment.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<RegisterDependencies>();

    containerBuilder.RegisterType<ShipmentQueryRepository>()
        .As<IShipmentQueryRepository>()
        .InstancePerLifetimeScope();

    containerBuilder.RegisterType<Repository>()
        .AsSelf()
        .InstancePerLifetimeScope();
});

builder.Services.AddDbContext<CommandDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CommandDb")));

builder.Services.AddDbContext<ReadDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReadDb")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<Shipment.Application.Commands.CreateShipmentCommand>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
