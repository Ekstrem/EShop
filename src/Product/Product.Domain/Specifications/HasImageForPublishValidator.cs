namespace Product.Domain.Specifications;

using Product.Domain.Abstraction;

internal sealed class HasImageForPublishValidator
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Media.Count >= 1;

    public string Reason => "Product must have at least one image to be published.";
}
