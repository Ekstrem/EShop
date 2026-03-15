namespace Invoice.Domain.Specifications;

using Invoice.Domain.Abstraction;

internal sealed class IsGeneratedValidator
{
    public bool IsSatisfiedBy(IInvoiceAnemicModel model)
        => model.Root.Status == "Generated";

    public string Reason => "Invoice must be in Generated status.";
}
