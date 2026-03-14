using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Category.DomainServices;
using Category.Storage;

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
    cfg.RegisterServicesFromAssemblyContaining<Category.Application.Commands.CreateCategoryCommand>());

builder.Services.AddDbContext<CommandDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CommandDb")));

builder.Services.AddDbContext<CategoryReadDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReadDb")));

builder.Services.AddScoped<Category.InternalContracts.ICategoryQueryRepository, CategoryRepository>();

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
