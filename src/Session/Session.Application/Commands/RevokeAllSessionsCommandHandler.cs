using MediatR;
using Session.Domain.Abstraction;
using Session.Domain.Implementation;
using Session.DomainServices;
using Session.InternalContracts;

namespace Session.Application.Commands;

public sealed class RevokeAllSessionsCommandHandler
    : IRequestHandler<RevokeAllSessionsCommand, int>
{
    private readonly AggregateProvider _provider;
    private readonly Notifier _notifier;
    private readonly ISessionQueryRepository _queryRepository;

    public RevokeAllSessionsCommandHandler(
        AggregateProvider provider,
        Notifier notifier,
        ISessionQueryRepository queryRepository)
    {
        _provider = provider;
        _notifier = notifier;
        _queryRepository = queryRepository;
    }

    public async Task<int> Handle(RevokeAllSessionsCommand request, CancellationToken ct)
    {
        var activeSessions = await _queryRepository.GetActiveByCustomerIdAsync(
            request.CustomerId, ct);

        var anemicModels = new List<ISessionAnemicModel>();
        foreach (var session in activeSessions)
        {
            var model = await _provider.GetByIdAsync(session.Id, ct);
            if (model is not null)
                anemicModels.Add(model);
        }

        var results = SessionAggregate.RevokeAllSessions(anemicModels.AsReadOnly());
        foreach (var result in results)
        {
            await _notifier.HandleAsync(result, ct);
        }

        return results.Count;
    }
}
