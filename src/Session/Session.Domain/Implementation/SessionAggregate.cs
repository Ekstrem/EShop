namespace Session.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Session.Domain.Abstraction;
using EShop.Contracts;

/// <summary>
/// Session aggregate containing all business operations.
/// Each method returns an AggregateResult for event-driven processing.
/// </summary>
public sealed class SessionAggregate
{
    private SessionAggregate() { }

    private static AggregateResult<ISession, ISessionAnemicModel> Success(
        ISessionAnemicModel oldModel, ISessionAnemicModel newModel)
    {
        var data = BusinessOperationData<ISession, ISessionAnemicModel>
            .Commit<ISession, ISessionAnemicModel>(oldModel, newModel);
        return new AggregateResultSuccess<ISession, ISessionAnemicModel>(data);
    }

    private static AggregateResult<ISession, ISessionAnemicModel> Fail(
        ISessionAnemicModel model, string error)
    {
        var data = BusinessOperationData<ISession, ISessionAnemicModel>
            .Commit<ISession, ISessionAnemicModel>(model, model);
        return new AggregateResultException<ISession, ISessionAnemicModel>(
            data, new FailedSpecification<ISession, ISessionAnemicModel>(error));
    }

    /// <summary>
    /// Creates a new session for a customer with valid credentials and Active account.
    /// </summary>
    public static AggregateResult<ISession, ISessionAnemicModel> CreateSession(
        Guid customerId,
        string token,
        TimeSpan duration,
        string deviceInfo)
    {
        var id = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var root = SessionRoot.CreateInstance(
            id, customerId, token, now.Add(duration), "Active", now, deviceInfo);
        var model = SessionAnemicModel.CreateInstance(id, root);

        var emptyRoot = SessionRoot.CreateInstance(
            Guid.Empty, Guid.Empty, string.Empty, DateTime.MinValue, string.Empty, DateTime.MinValue, string.Empty);
        var empty = SessionAnemicModel.CreateInstance(Guid.Empty, emptyRoot);
        return Success(empty, model);
    }

    /// <summary>
    /// Refreshes an active, non-expired session by extending its expiration.
    /// </summary>
    public static AggregateResult<ISession, ISessionAnemicModel> RefreshSession(
        ISessionAnemicModel current,
        string newToken,
        TimeSpan duration)
    {
        var root = SessionRoot.CreateInstance(
            current.Root.Id,
            current.Root.CustomerId,
            newToken,
            DateTime.UtcNow.Add(duration),
            "Active",
            current.Root.CreatedAt,
            current.Root.DeviceInfo);
        var model = SessionAnemicModel.CreateInstance(current.Id, root);

        return Success(current, model);
    }

    /// <summary>
    /// Revokes a single active session.
    /// </summary>
    public static AggregateResult<ISession, ISessionAnemicModel> RevokeSession(
        ISessionAnemicModel current)
    {
        var root = SessionRoot.CreateInstance(
            current.Root.Id,
            current.Root.CustomerId,
            current.Root.Token,
            current.Root.ExpiresAt,
            "Revoked",
            current.Root.CreatedAt,
            current.Root.DeviceInfo);
        var model = SessionAnemicModel.CreateInstance(current.Id, root);

        return Success(current, model);
    }

    /// <summary>
    /// Revokes all active sessions for a customer.
    /// Returns the result for the last revoked session (caller should iterate).
    /// </summary>
    public static IReadOnlyList<AggregateResult<ISession, ISessionAnemicModel>> RevokeAllSessions(
        IReadOnlyList<ISessionAnemicModel> activeSessions)
    {
        return activeSessions
            .Select(session =>
            {
                var root = SessionRoot.CreateInstance(
                    session.Root.Id,
                    session.Root.CustomerId,
                    session.Root.Token,
                    session.Root.ExpiresAt,
                    "Revoked",
                    session.Root.CreatedAt,
                    session.Root.DeviceInfo);
                var model = SessionAnemicModel.CreateInstance(session.Id, root);
                return Success(session, model);
            })
            .ToList()
            .AsReadOnly();
    }
}
