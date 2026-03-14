using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Invoice.Domain;
using Invoice.Domain.Abstraction;
using Invoice.DomainServices;
using Invoice.InternalContracts;
using Invoice.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<RegisterDependencies>();

    containerBuilder.RegisterType<Repository>()
        .As<IQueryRepository<IInvoice, IInvoiceAnemicModel>>()
        .InstancePerLifetimeScope();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<Invoice.Application.Commands.GenerateInvoiceCommand>());

builder.Services.AddDbContext<CommandDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CommandDb")));

builder.Services.AddDbContext<ReadDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReadDb")));

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
