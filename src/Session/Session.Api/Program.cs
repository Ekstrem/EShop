using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hive.SeedWorks.TacticalPatterns;
using Microsoft.EntityFrameworkCore;
using Session.Domain;
using Session.Domain.Abstraction;
using Session.DomainServices;
using Session.InternalContracts;
using Session.Storage;
using ISession = Session.Domain.ISession;

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
    cfg.RegisterServicesFromAssemblyContaining<Session.Application.Commands.CreateSessionCommand>());

// EF Core - Command DbContext
builder.Services.AddDbContext<CommandDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CommandDb")));

// EF Core - Read DbContext
builder.Services.AddDbContext<SessionReadDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReadDb")));

// Register read repository.
builder.Services.AddScoped<ISessionQueryRepository>(sp =>
    sp.GetRequiredService<SessionReadDbContext>());

// Register command repository.
builder.Services.AddScoped<IRepository<ISession, ISessionAnemicModel>, SessionRepository>();

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
