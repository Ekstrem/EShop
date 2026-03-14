namespace Invoice.Domain.Specifications;

using Invoice.Domain.Abstraction;

internal sealed class HasRequiredFieldsValidator
{
    public bool IsSatisfiedBy(
        string invoiceNumber,
        Guid customerId,
        IReadOnlyList<IInvoiceLine> lines)
    {
        if (string.IsNullOrWhiteSpace(invoiceNumber))
            return false;

        if (customerId == Guid.Empty)
            return false;

        if (lines is null || lines.Count == 0)
            return false;

        if (lines.Any(l => l.VatRate < 0))
            return false;

        return true;
    }

    public string ErrorMessage => "Invoice must have a number, date, customer, at least one line, and valid VAT rates.";
}
