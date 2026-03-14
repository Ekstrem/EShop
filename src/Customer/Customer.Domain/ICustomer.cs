using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain;

/// <summary>
/// Bounded context marker for the Customer context.
/// </summary>
public interface ICustomer : IBoundedContext
{
}

/// <summary>
/// Describes the Customer bounded context.
/// </summary>
public sealed class CustomerBoundedContextDescription : IBoundedContextDescription
{
    public string Name => "Customer";

    public string Description =>
        "Manages customer registration, profile, addresses, consents, and account lifecycle.";
}
