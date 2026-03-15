namespace DiscountCode.Domain.Specifications;

using System.Text.RegularExpressions;
using DiscountCode.Domain.Abstraction;

internal sealed class CodeFormatValidator
{
    private static readonly Regex CodePattern = new("^[A-Z0-9]{6,20}$", RegexOptions.Compiled);

    public bool IsSatisfiedBy(IDiscountCodeAnemicModel model)
        => CodePattern.IsMatch(model.Root.Code);

    public string Reason => "Code must be 6-20 characters, uppercase alphanumeric only.";
}
