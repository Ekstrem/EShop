namespace Invoice.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Invoice.Domain.Abstraction;

internal sealed class InvoiceImmutableValidator : IBusinessOperationValidator<IInvoiceAnemicModel>
{
    public bool IsSatisfiedBy(IInvoiceAnemicModel model)
        => model.Root.Status == "Generated" || model.Root.Status == "Sent";

    public string ErrorMessage => "Invoice cannot be modified after it has been generated.";
}
