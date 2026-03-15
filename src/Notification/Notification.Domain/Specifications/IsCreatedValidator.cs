namespace Notification.Domain.Specifications;

using Notification.Domain.Abstraction;

internal sealed class IsCreatedValidator
{
    public bool IsSatisfiedBy(INotificationAnemicModel model)
        => model.Root.Status == "Created";

    public string Reason => "Notification must be in Created status.";
}
