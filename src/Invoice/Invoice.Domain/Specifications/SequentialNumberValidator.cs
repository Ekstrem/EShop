namespace Invoice.Domain.Specifications;

using System.Text.RegularExpressions;

internal sealed class SequentialNumberValidator
{
    private static readonly Regex NumberPattern = new(@"^(INV|CN)-\d{6,}$", RegexOptions.Compiled);

    public bool IsSatisfiedBy(string invoiceNumber)
        => !string.IsNullOrWhiteSpace(invoiceNumber) && NumberPattern.IsMatch(invoiceNumber);

    public string Reason => "Invoice number must follow the sequential format (e.g., INV-000001 or CN-000001).";
}
