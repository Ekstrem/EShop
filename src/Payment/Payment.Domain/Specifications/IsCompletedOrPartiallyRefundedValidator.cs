namespace Payment.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Payment.Domain.Abstraction;

internal sealed class IsCompletedOrPartiallyRefundedValidator : IBusinessOperationValidator<IPaymentAnemicModel>
{
    public bool IsSatisfiedBy(IPaymentAnemicModel model)
        => model.Root.Status == "Completed" || model.Root.Status == "PartiallyRefunded";

    public string ErrorMessage => "Payment must be in Completed or PartiallyRefunded status.";
}
