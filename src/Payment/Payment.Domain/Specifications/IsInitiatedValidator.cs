namespace Payment.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Payment.Domain.Abstraction;

internal sealed class IsInitiatedValidator : IBusinessOperationValidator<IPaymentAnemicModel>
{
    public bool IsSatisfiedBy(IPaymentAnemicModel model)
        => model.Root.Status == "Initiated";

    public string ErrorMessage => "Payment must be in Initiated status.";
}
