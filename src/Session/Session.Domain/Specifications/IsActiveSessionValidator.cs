namespace Session.Domain.Specifications;

using Session.Domain.Abstraction;

/// <summary>
/// Validates that the session is in Active status.
/// </summary>
public sealed class IsActiveSessionValidator
{
    private IsActiveSessionValidator() { }

    public static IsActiveSessionValidator CreateInstance()
    {
        return new IsActiveSessionValidator();
    }

    public bool IsSatisfiedBy(ISessionAnemicModel model)
    {
        return model.Root.Status == "Active";
    }

    public string Reason => "Session must be in Active status.";
}
