using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request is in Received status.
/// </summary>
public sealed class IsReceivedValidator : IBusinessOperationValidator<IReturnRequest, IReturnRequestAnemicModel>
{
    public bool IsSatisfiedBy(IReturnRequestAnemicModel model)
        => model.Root.Status == "Received";

    public string ErrorMessage => "Return request must be in Received status.";
}
