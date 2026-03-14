using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StockItem.DomainServices;
using StockItem.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<RegisterDependencies>();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<StockItem.Application.Commands.ReserveStockCommand>());

builder.Services.AddDbContext<CommandDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CommandDb")));

builder.Services.AddDbContext<StockItemReadDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReadDb")));

builder.Services.AddScoped<StockItem.InternalContracts.IStockItemQueryRepository, StockItemRepository>();

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
