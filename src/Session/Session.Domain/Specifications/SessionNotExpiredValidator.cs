namespace Session.Domain.Specifications;

using Session.Domain.Abstraction;

/// <summary>
/// Validates that the session has not expired.
/// </summary>
public sealed class SessionNotExpiredValidator
{
    private SessionNotExpiredValidator() { }

    public static SessionNotExpiredValidator CreateInstance()
    {
        return new SessionNotExpiredValidator();
    }

    public bool IsSatisfiedBy(ISessionAnemicModel model)
    {
        return model.Root.ExpiresAt > DateTime.UtcNow;
    }

    public string Reason => "Session has expired.";
}
