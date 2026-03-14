namespace Invoice.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Invoice.Domain.Abstraction;

internal sealed class IsSentValidator : IBusinessOperationValidator<IInvoiceAnemicModel>
{
    public bool IsSatisfiedBy(IInvoiceAnemicModel model)
        => model.Root.Status == "Sent";

    public string ErrorMessage => "Invoice must be in Sent status.";
}
