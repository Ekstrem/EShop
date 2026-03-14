using Hive.SeedWorks.TacticalPatterns;
using Session.Domain.Abstraction;

namespace Session.Domain.Specifications;

/// <summary>
/// Validates that the session has not expired.
/// </summary>
public sealed class SessionNotExpiredValidator : IBusinessOperationValidator<ISession, ISessionAnemicModel>
{
    private SessionNotExpiredValidator()
    {
    }

    public static SessionNotExpiredValidator CreateInstance()
    {
        return new SessionNotExpiredValidator();
    }

    public bool IsSatisfiedBy(ISessionAnemicModel model)
    {
        return model.Root.ExpiresAt > DateTime.UtcNow;
    }

    public string ErrorMessage => "Session has expired.";
}
