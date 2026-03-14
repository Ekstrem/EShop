using Hive.SeedWorks.TacticalPatterns;
using Session.Domain.Abstraction;

namespace Session.Domain.Specifications;

/// <summary>
/// Validates that the session is in Active status.
/// </summary>
public sealed class IsActiveSessionValidator : IBusinessOperationValidator<ISession, ISessionAnemicModel>
{
    private IsActiveSessionValidator()
    {
    }

    public static IsActiveSessionValidator CreateInstance()
    {
        return new IsActiveSessionValidator();
    }

    public bool IsSatisfiedBy(ISessionAnemicModel model)
    {
        return model.Root.Status == "Active";
    }

    public string ErrorMessage => "Session must be in Active status.";
}
