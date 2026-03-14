namespace DiscountCode.Domain.Specifications;

using System.Text.RegularExpressions;
using Hive.SeedWorks.TacticalPatterns;
using DiscountCode.Domain.Abstraction;

internal sealed class CodeFormatValidator : IBusinessOperationValidator<IDiscountCodeAnemicModel>
{
    private static readonly Regex CodePattern = new("^[A-Z0-9]{6,20}$", RegexOptions.Compiled);

    public bool IsSatisfiedBy(IDiscountCodeAnemicModel model)
        => CodePattern.IsMatch(model.Root.Code);

    public string ErrorMessage => "Code must be 6-20 characters, uppercase alphanumeric only.";
}
