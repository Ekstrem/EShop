using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
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
