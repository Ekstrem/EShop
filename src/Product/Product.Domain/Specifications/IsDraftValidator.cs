namespace Product.Domain.Specifications;

using Product.Domain.Abstraction;

internal sealed class IsDraftValidator
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Root.Status == "Draft";

    public string Reason => "Product must be in Draft status.";
}
