using Hive.SeedWorks.TacticalPatterns;

namespace ReturnRequest.Domain;

/// <summary>
/// Bounded context marker for the ReturnRequest context.
/// </summary>
public interface IReturnRequest : IBoundedContext
{
}

/// <summary>
/// Describes the ReturnRequest bounded context.
/// </summary>
public sealed class ReturnRequestBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "ReturnRequest";
    public int MicroserviceVersion => 1;
}
