using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request is in Approved status.
/// </summary>
internal sealed class IsApprovedValidator
{
    public bool IsSatisfiedBy(IReturnRequestAnemicModel model)
        => model.Root.Status == "Approved";

    public string Reason => "Return request must be in Approved status.";
}
