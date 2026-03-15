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
    public string ContextName => "Customer";
    public int MicroserviceVersion => 1;
}
