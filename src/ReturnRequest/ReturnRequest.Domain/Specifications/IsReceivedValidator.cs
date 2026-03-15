using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request is in Received status.
/// </summary>
internal sealed class IsReceivedValidator
{
    public bool IsSatisfiedBy(IReturnRequestAnemicModel model)
        => model.Root.Status == "Received";

    public string Reason => "Return request must be in Received status.";
}
