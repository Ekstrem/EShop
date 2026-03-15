namespace Product.Domain.Specifications;

using Product.Domain.Abstraction;

internal sealed class IsPublishedValidator
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Root.Status == "Published";

    public string Reason => "Product must be in Published status.";
}
