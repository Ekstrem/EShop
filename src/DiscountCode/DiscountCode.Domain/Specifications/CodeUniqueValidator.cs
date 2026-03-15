namespace DiscountCode.Domain.Specifications;

using DiscountCode.Domain.Abstraction;

internal sealed class CodeUniqueValidator
{
    public bool IsSatisfiedBy(IDiscountCodeAnemicModel model)
        => !string.IsNullOrWhiteSpace(model.Root.Code);

    public string Reason => "Code must be unique.";
}
