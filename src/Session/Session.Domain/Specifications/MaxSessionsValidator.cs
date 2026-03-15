namespace Session.Domain.Specifications;

using Session.Domain.Abstraction;

/// <summary>
/// Validates that a customer does not exceed the maximum of 5 active sessions.
/// </summary>
public sealed class MaxSessionsValidator
{
    private const int MaxActiveSessions = 5;
    private readonly int _currentActiveSessionCount;

    private MaxSessionsValidator(int currentActiveSessionCount)
    {
        _currentActiveSessionCount = currentActiveSessionCount;
    }

    public static MaxSessionsValidator CreateInstance(int currentActiveSessionCount)
    {
        return new MaxSessionsValidator(currentActiveSessionCount);
    }

    public bool IsSatisfiedBy(ISessionAnemicModel model)
    {
        return _currentActiveSessionCount < MaxActiveSessions;
    }

    public string Reason => $"Customer cannot have more than {MaxActiveSessions} active sessions.";
}
