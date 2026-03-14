namespace Notification.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Notification.Domain.Abstraction;

internal sealed class TemplateExistsValidator : IBusinessOperationValidator<INotificationAnemicModel>
{
    public bool IsSatisfiedBy(INotificationAnemicModel model)
        => !string.IsNullOrWhiteSpace(model.Root.TemplateId);

    public string ErrorMessage => "Template does not exist.";
}
