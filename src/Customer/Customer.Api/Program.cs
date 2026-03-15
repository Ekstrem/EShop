using Autofac;
using Autofac.Extensions.DependencyInjection;
using Customer.Domain;
using Customer.Domain.Abstraction;
using Customer.DomainServices;
using Customer.InternalContracts;
using Customer.Storage;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Use Autofac as the DI container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<RegisterDependencies>();
});

// Add services.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<Customer.Application.Commands.RegisterCustomerCommand>());

// EF Core - Command DbContext
builder.Services.AddDbContext<CommandDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CommandDb")));

// EF Core - Read DbContext
builder.Services.AddDbContext<CustomerReadDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReadDb")));

// Register read repository.
builder.Services.AddScoped<ICustomerQueryRepository>(sp =>
    sp.GetRequiredService<CustomerReadDbContext>());

// Register command repository.
builder.Services.AddScoped<IRepository<ICustomer, ICustomerAnemicModel>, CustomerRepository>();

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
