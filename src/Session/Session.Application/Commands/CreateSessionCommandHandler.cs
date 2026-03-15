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
using Session.InternalContracts;

namespace Session.Application.Commands;

public sealed class CreateSessionCommandHandler
    : IRequestHandler<CreateSessionCommand, AggregateResult<ISession, ISessionAnemicModel>>
{
    private readonly Notifier _notifier;
    private readonly ISessionQueryRepository _queryRepository;

    public CreateSessionCommandHandler(
        Notifier notifier,
        ISessionQueryRepository queryRepository)
    {
        _notifier = notifier;
        _queryRepository = queryRepository;
    }

    public async Task<AggregateResult<ISession, ISessionAnemicModel>> Handle(
        CreateSessionCommand request,
        CancellationToken ct)
    {
        // Validate login attempts.
        var failedCount = await _queryRepository.GetFailedLoginAttemptsAsync(
            request.CustomerId, TimeSpan.FromMinutes(15), ct);
        var lastFailed = await _queryRepository.GetLastFailedLoginAttemptAsync(
            request.CustomerId, ct);
        var loginValidator = LoginAttemptsValidator.CreateInstance(failedCount, lastFailed);

        // Create a temporary model for validation.
        var tempRoot = SessionRoot.CreateInstance(
            Guid.Empty, request.CustomerId, request.Token,
            DateTime.UtcNow.AddMinutes(request.DurationMinutes),
            "Active", DateTime.UtcNow, request.DeviceInfo);
        var tempModel = SessionAnemicModel.CreateInstance(Guid.Empty, tempRoot);

        if (!loginValidator.IsSatisfiedBy(tempModel))
            throw new InvalidOperationException(loginValidator.Reason);

        // Validate max sessions.
        var activeCount = await _queryRepository.GetActiveSessionCountAsync(
            request.CustomerId, ct);
        var maxValidator = MaxSessionsValidator.CreateInstance(activeCount);
        if (!maxValidator.IsSatisfiedBy(tempModel))
            throw new InvalidOperationException(maxValidator.Reason);

        var result = SessionAggregate.CreateSession(
            request.CustomerId,
            request.Token,
            TimeSpan.FromMinutes(request.DurationMinutes),
            request.DeviceInfo);

        await _notifier.HandleAsync(result, ct);
        return result;
    }
}
