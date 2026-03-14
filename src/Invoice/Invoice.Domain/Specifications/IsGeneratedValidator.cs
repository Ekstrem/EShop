namespace Invoice.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Invoice.Domain.Abstraction;

internal sealed class IsGeneratedValidator : IBusinessOperationValidator<IInvoiceAnemicModel>
{
    public bool IsSatisfiedBy(IInvoiceAnemicModel model)
        => model.Root.Status == "Generated";

    public string ErrorMessage => "Invoice must be in Generated status.";
}
