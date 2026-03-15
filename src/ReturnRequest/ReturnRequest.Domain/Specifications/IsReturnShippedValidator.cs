using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request is in ReturnShipped status.
/// </summary>
internal sealed class IsReturnShippedValidator
{
    public bool IsSatisfiedBy(IReturnRequestAnemicModel model)
        => model.Root.Status == "ReturnShipped";

    public string Reason => "Return request must be in ReturnShipped status.";
}
