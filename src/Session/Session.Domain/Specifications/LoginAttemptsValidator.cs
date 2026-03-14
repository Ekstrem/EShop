using Hive.SeedWorks.TacticalPatterns;
using Session.Domain.Abstraction;

namespace Session.Domain.Specifications;

/// <summary>
/// Validates that the customer has not exceeded 5 failed login attempts
/// within the last 15 minutes, blocking further attempts if so.
/// </summary>
public sealed class LoginAttemptsValidator : IBusinessOperationValidator<ISession, ISessionAnemicModel>
{
    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan BlockDuration = TimeSpan.FromMinutes(15);

    private readonly int _failedAttemptCount;
    private readonly DateTime? _lastFailedAttemptAt;

    private LoginAttemptsValidator(int failedAttemptCount, DateTime? lastFailedAttemptAt)
    {
        _failedAttemptCount = failedAttemptCount;
        _lastFailedAttemptAt = lastFailedAttemptAt;
    }

    public static LoginAttemptsValidator CreateInstance(
        int failedAttemptCount,
        DateTime? lastFailedAttemptAt)
    {
        return new LoginAttemptsValidator(failedAttemptCount, lastFailedAttemptAt);
    }

    public bool IsSatisfiedBy(ISessionAnemicModel model)
    {
        if (_failedAttemptCount < MaxFailedAttempts)
            return true;

        if (_lastFailedAttemptAt is null)
            return true;

        var blockExpiresAt = _lastFailedAttemptAt.Value.Add(BlockDuration);
        return DateTime.UtcNow >= blockExpiresAt;
    }

    public string ErrorMessage =>
        $"Account is temporarily blocked due to {MaxFailedAttempts} failed login attempts. " +
        $"Please try again after {BlockDuration.TotalMinutes} minutes.";
}
