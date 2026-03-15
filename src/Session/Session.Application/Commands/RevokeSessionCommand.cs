using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using Session.Domain;
using Session.Domain.Abstraction;

namespace Session.Application.Commands;

/// <summary>
/// Command to revoke a single session (logout).
/// </summary>
public sealed class RevokeSessionCommand : IRequest<AggregateResult<ISession, ISessionAnemicModel>>
{
    public Guid SessionId { get; init; }
}
