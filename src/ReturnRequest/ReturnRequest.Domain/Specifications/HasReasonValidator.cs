using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request has a reason provided.
/// </summary>
public sealed class HasReasonValidator : IBusinessOperationValidator<IReturnRequest, IReturnRequestAnemicModel>
{
    public bool IsSatisfiedBy(IReturnRequestAnemicModel model)
        => !string.IsNullOrWhiteSpace(model.Root.Reason);

    public string ErrorMessage => "Return request must have a reason.";
}
