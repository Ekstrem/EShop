using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request is in Approved status.
/// </summary>
public sealed class IsApprovedValidator : IBusinessOperationValidator<IReturnRequest, IReturnRequestAnemicModel>
{
    public bool IsSatisfiedBy(IReturnRequestAnemicModel model)
        => model.Root.Status == "Approved";

    public string ErrorMessage => "Return request must be in Approved status.";
}
