namespace Notification.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using Notification.Domain.Abstraction;
using Notification.Domain.Specifications;

internal sealed class Aggregate : Aggregate<INotification, INotificationAnemicModel>
{
    private Aggregate(INotificationAnemicModel model) : base(model) { }

    public static Aggregate CreateInstance(INotificationAnemicModel model) => new(model);

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
            return AggregateResult<INotification, INotificationAnemicModel>.Fail(
                "Notification for this eventId and customerId already exists.");

        var templateValidator = new TemplateExistsValidator();
        if (!templateValidator.IsSatisfiedBy(anemic))
            return AggregateResult<INotification, INotificationAnemicModel>.Fail(
                "Template does not exist.");

        return AggregateResult<INotification, INotificationAnemicModel>.Ok(anemic);
    }

    public AggregateResult<INotification, INotificationAnemicModel> Render(
        string renderedContent,
        string subject)
    {
        var validator = new IsCreatedValidator();
        if (!validator.IsSatisfiedBy(Model))
            return AggregateResult<INotification, INotificationAnemicModel>.Fail(
                "Only notifications in Created status can be rendered.");

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

        return AggregateResult<INotification, INotificationAnemicModel>.Ok(anemic);
    }

    public AggregateResult<INotification, INotificationAnemicModel> Send(bool hasConsent)
    {
        var validator = new IsRenderedValidator();
        if (!validator.IsSatisfiedBy(Model))
            return AggregateResult<INotification, INotificationAnemicModel>.Fail(
                "Only notifications in Rendered status can be sent.");

        var consentValidator = new ConsentValidator();
        if (Model.Root.Type == "Marketing" && !hasConsent)
            return AggregateResult<INotification, INotificationAnemicModel>.Fail(
                "Marketing notifications require customer consent.");

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

        return AggregateResult<INotification, INotificationAnemicModel>.Ok(anemic);
    }

    public AggregateResult<INotification, INotificationAnemicModel> Retry()
    {
        var maxRetry = new MaxRetryValidator();
        if (!maxRetry.IsSatisfiedBy(Model))
            return AggregateResult<INotification, INotificationAnemicModel>.Fail(
                "Maximum retry count (3) exceeded.");

        if (Model.Root.Status != "Failed")
            return AggregateResult<INotification, INotificationAnemicModel>.Fail(
                "Only failed notifications can be retried.");

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

        return AggregateResult<INotification, INotificationAnemicModel>.Ok(anemic);
    }
}
