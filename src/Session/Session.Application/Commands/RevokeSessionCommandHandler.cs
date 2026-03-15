using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using MediatR;
using Session.Domain;
using Session.Domain.Abstraction;
using Session.Domain.Implementation;
using Session.Domain.Specifications;
using Session.DomainServices;

namespace Session.Application.Commands;

public sealed class RevokeSessionCommandHandler
    : IRequestHandler<RevokeSessionCommand, AggregateResult<ISession, ISessionAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly Notifier _notifier;

    public RevokeSessionCommandHandler(AggregateProvider provider, Notifier notifier)
    {
        _provider = provider;
        _notifier = notifier;
    }

    public async Task<AggregateResult<ISession, ISessionAnemicModel>> Handle(
        RevokeSessionCommand request,
        CancellationToken ct)
    {
        var current = await _provider.GetByIdAsync(request.SessionId, ct)
            ?? throw new InvalidOperationException("Session not found.");

        var activeValidator = IsActiveSessionValidator.CreateInstance();
        if (!activeValidator.IsSatisfiedBy(current))
            throw new InvalidOperationException(activeValidator.Reason);

        var result = SessionAggregate.RevokeSession(current);
        await _notifier.HandleAsync(result, ct);
        return result;
    }
}
