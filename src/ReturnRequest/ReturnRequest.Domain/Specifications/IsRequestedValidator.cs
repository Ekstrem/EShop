using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request is in Requested status.
/// </summary>
public sealed class IsRequestedValidator : IBusinessOperationValidator<IReturnRequest, IReturnRequestAnemicModel>
{
    public bool IsValid(IReturnRequestAnemicModel model)
        => model.Root.Status == "Requested";

    public string ErrorMessage => "Return request must be in Requested status.";
}
