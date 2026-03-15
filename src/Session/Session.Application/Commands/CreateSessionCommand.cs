using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using Session.Domain;
using Session.Domain.Abstraction;

namespace Session.Application.Commands;

/// <summary>
/// Command to create a new session (login).
/// </summary>
public sealed class CreateSessionCommand : IRequest<AggregateResult<ISession, ISessionAnemicModel>>
{
    public Guid CustomerId { get; init; }
    public string Token { get; init; } = string.Empty;
    public string DeviceInfo { get; init; } = string.Empty;
    public int DurationMinutes { get; init; } = 60;
}
