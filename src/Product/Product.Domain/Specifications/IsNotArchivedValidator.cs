namespace Product.Domain.Specifications;

using Product.Domain.Abstraction;

internal sealed class IsNotArchivedValidator
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Root.Status != "Archived";

    public string Reason => "Product must not be archived for this operation.";
}
