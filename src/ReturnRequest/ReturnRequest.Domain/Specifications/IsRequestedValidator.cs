using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request is in Requested status.
/// </summary>
internal sealed class IsRequestedValidator
{
    public bool IsSatisfiedBy(IReturnRequestAnemicModel model)
        => model.Root.Status == "Requested";

    public string Reason => "Return request must be in Requested status.";
}
