namespace Notification.Domain.Specifications;

using Notification.Domain.Abstraction;

internal sealed class IsRenderedValidator
{
    public bool IsSatisfiedBy(INotificationAnemicModel model)
        => model.Root.Status == "Rendered";

    public string Reason => "Notification must be in Rendered status.";
}
