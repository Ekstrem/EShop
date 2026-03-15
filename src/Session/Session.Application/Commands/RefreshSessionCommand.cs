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
/// Command to refresh an existing session.
/// </summary>
public sealed class RefreshSessionCommand : IRequest<AggregateResult<ISession, ISessionAnemicModel>>
{
    public Guid SessionId { get; init; }
    public string NewToken { get; init; } = string.Empty;
    public int DurationMinutes { get; init; } = 60;
}
