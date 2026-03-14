namespace DiscountCode.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using DiscountCode.Domain.Abstraction;

internal sealed class CodeUniqueValidator : IBusinessOperationValidator<IDiscountCodeAnemicModel>
{
    public bool IsSatisfiedBy(IDiscountCodeAnemicModel model)
        => !string.IsNullOrWhiteSpace(model.Root.Code);

    public string ErrorMessage => "Code must be unique.";
}
