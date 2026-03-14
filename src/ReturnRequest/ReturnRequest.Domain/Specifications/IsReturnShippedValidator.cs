using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request is in ReturnShipped status.
/// </summary>
public sealed class IsReturnShippedValidator : IBusinessOperationValidator<IReturnRequest, IReturnRequestAnemicModel>
{
    public bool IsValid(IReturnRequestAnemicModel model)
        => model.Root.Status == "ReturnShipped";

    public string ErrorMessage => "Return request must be in ReturnShipped status.";
}
