namespace Product.Domain.Specifications;

using Product.Domain.Abstraction;

internal sealed class IsArchivedValidator
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Root.Status == "Archived";

    public string Reason => "Product must be in Archived status.";
}
