namespace Notification.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Notification.Domain.Abstraction;
using Notification.Domain.Specifications;
using EShop.Contracts;

internal sealed class Aggregate
{
    public INotificationAnemicModel Model { get; }

    private Aggregate(INotificationAnemicModel model) => Model = model;

    public static Aggregate CreateInstance(INotificationAnemicModel model) => new(model);

    private AggregateResult<INotification, INotificationAnemicModel> Success(INotificationAnemicModel newModel)
    {
        var data = BusinessOperationData<INotification, INotificationAnemicModel>
            .Commit<INotification, INotificationAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<INotification, INotificationAnemicModel>(data);
    }

    private AggregateResult<INotification, INotificationAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<INotification, INotificationAnemicModel>
            .Commit<INotification, INotificationAnemicModel>(Model, Model);
        return new AggregateResultException<INotification, INotificationAnemicModel>(
            data, new FailedSpecification<INotification, INotificationAnemicModel>(error));
    }

    public AggregateResult<INotification, INotificationAnemicModel> CreateNotification(
        Guid customerId,
        Guid eventId,
        string channel,
        string templateId,
        string locale,
        string type)
    {
        var root = NotificationRoot.CreateInstance(
            customerId, eventId, channel, templateId, locale, type);

        var anemic = new AnemicModel
        {
            Root = root,
            RenderedContent = string.Empty,
            Subject = string.Empty
        };

        var dedup = new DeduplicationValidator();
        if (!dedup.IsSatisfiedBy(anemic))
            return Fail("Notification for this eventId and customerId already exists.");

        var templateValidator = new TemplateExistsValidator();
        if (!templateValidator.IsSatisfiedBy(anemic))
            return Fail("Template does not exist.");

        return Success(anemic);
    }

    public AggregateResult<INotification, INotificationAnemicModel> Render(
        string renderedContent,
        string subject)
    {
        var validator = new IsCreatedValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Only notifications in Created status can be rendered.");

        var root = NotificationRoot.CreateInstance(
            Model.Root.CustomerId,
            Model.Root.EventId,
            Model.Root.Channel,
            Model.Root.TemplateId,
            Model.Root.Locale,
            Model.Root.Type,
            "Rendered",
            Model.Root.RetryCount,
            Model.Root.CreatedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            RenderedContent = renderedContent,
            Subject = subject
        };

        return Success(anemic);
    }

    public AggregateResult<INotification, INotificationAnemicModel> Send(bool hasConsent)
    {
        var validator = new IsRenderedValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Only notifications in Rendered status can be sent.");

        var consentValidator = new ConsentValidator();
        if (Model.Root.Type == "Marketing" && !hasConsent)
            return Fail("Marketing notifications require customer consent.");

        var root = NotificationRoot.CreateInstance(
            Model.Root.CustomerId,
            Model.Root.EventId,
            Model.Root.Channel,
            Model.Root.TemplateId,
            Model.Root.Locale,
            Model.Root.Type,
            "Sent",
            Model.Root.RetryCount,
            Model.Root.CreatedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            RenderedContent = Model.RenderedContent,
            Subject = Model.Subject
        };

        return Success(anemic);
    }

    public AggregateResult<INotification, INotificationAnemicModel> Retry()
    {
        var maxRetry = new MaxRetryValidator();
        if (!maxRetry.IsSatisfiedBy(Model))
            return Fail("Maximum retry count (3) exceeded.");

        if (Model.Root.Status != "Failed")
            return Fail("Only failed notifications can be retried.");

        var root = NotificationRoot.CreateInstance(
            Model.Root.CustomerId,
            Model.Root.EventId,
            Model.Root.Channel,
            Model.Root.TemplateId,
            Model.Root.Locale,
            Model.Root.Type,
            "Created",
            Model.Root.RetryCount + 1,
            Model.Root.CreatedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            RenderedContent = string.Empty,
            Subject = string.Empty
        };

        return Success(anemic);
    }
}
