namespace Notification.Domain.Specifications;

using Notification.Domain.Abstraction;

internal sealed class TemplateExistsValidator
{
    public bool IsSatisfiedBy(INotificationAnemicModel model)
        => !string.IsNullOrWhiteSpace(model.Root.TemplateId);

    public string Reason => "Template does not exist.";
}
