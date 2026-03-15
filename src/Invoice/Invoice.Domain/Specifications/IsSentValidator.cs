namespace Invoice.Domain.Specifications;

using Invoice.Domain.Abstraction;

internal sealed class IsSentValidator
{
    public bool IsSatisfiedBy(IInvoiceAnemicModel model)
        => model.Root.Status == "Sent";

    public string Reason => "Invoice must be in Sent status.";
}
