using Customer.Domain.Abstraction;

namespace Customer.Domain.Implementation;

/// <summary>
/// Immutable implementation of a consent value object.
/// </summary>
public sealed class Consent : IConsent
{
    private Consent(string consentType, bool isGranted, DateTime grantedAt)
    {
        ConsentType = consentType;
        IsGranted = isGranted;
        GrantedAt = grantedAt;
    }

    public string ConsentType { get; }
    public bool IsGranted { get; }
    public DateTime GrantedAt { get; }

    public static Consent CreateInstance(string consentType, bool isGranted, DateTime grantedAt)
    {
        return new Consent(consentType, isGranted, grantedAt);
    }
}
