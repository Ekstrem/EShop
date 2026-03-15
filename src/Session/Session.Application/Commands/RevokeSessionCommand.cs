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
/// Command to revoke a single session (logout).
/// </summary>
public sealed class RevokeSessionCommand : IRequest<AggregateResult<ISession, ISessionAnemicModel>>
{
    public Guid SessionId { get; init; }
}
