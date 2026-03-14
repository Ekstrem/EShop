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
    public string Name => "ReturnRequest";

    public string Description =>
        "Manages return requests, approvals, rejections, return shipping, receiving, and refund lifecycle.";
}
