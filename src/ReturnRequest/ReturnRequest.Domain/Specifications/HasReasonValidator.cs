using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request has a reason provided.
/// </summary>
internal sealed class HasReasonValidator
{
    public bool IsSatisfiedBy(IReturnRequestAnemicModel model)
        => !string.IsNullOrWhiteSpace(model.Root.Reason);

    public string Reason => "Return request must have a reason.";
}
