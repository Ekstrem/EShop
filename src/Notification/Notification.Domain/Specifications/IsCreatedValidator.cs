namespace Notification.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Notification.Domain.Abstraction;

internal sealed class IsCreatedValidator : IBusinessOperationValidator<INotificationAnemicModel>
{
    public bool IsSatisfiedBy(INotificationAnemicModel model)
        => model.Root.Status == "Created";

    public string ErrorMessage => "Notification must be in Created status.";
}
