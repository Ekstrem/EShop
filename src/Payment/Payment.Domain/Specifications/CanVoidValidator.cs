namespace Payment.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Payment.Domain.Abstraction;

internal sealed class CanVoidValidator : IBusinessOperationValidator<IPaymentAnemicModel>
{
    public bool IsSatisfiedBy(IPaymentAnemicModel model)
        => model.Root.Status == "Initiated";

    public string ErrorMessage => "Only payments in Initiated status can be voided.";
}
