namespace Notification.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Notification.Domain.Abstraction;

internal sealed class ConsentValidator : IBusinessOperationValidator<INotificationAnemicModel>
{
    public bool IsSatisfiedBy(INotificationAnemicModel model)
        => model.Root.Type != "Marketing";

    public string ErrorMessage => "Marketing notifications require customer consent.";
}
