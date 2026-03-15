namespace Invoice.Domain.Specifications;

using Invoice.Domain.Abstraction;

internal sealed class InvoiceImmutableValidator
{
    public bool IsSatisfiedBy(IInvoiceAnemicModel model)
        => model.Root.Status == "Generated" || model.Root.Status == "Sent";

    public string Reason => "Invoice cannot be modified after it has been generated.";
}
