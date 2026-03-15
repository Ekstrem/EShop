namespace Notification.Domain.Specifications;

using Notification.Domain.Abstraction;

internal sealed class ConsentValidator
{
    public bool IsSatisfiedBy(INotificationAnemicModel model)
        => model.Root.Type != "Marketing";

    public string Reason => "Marketing notifications require customer consent.";
}
