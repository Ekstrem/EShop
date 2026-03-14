namespace Notification.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface INotificationAnemicModel : IAnemicModel<INotification>
{
    INotificationRoot Root { get; }
    string RenderedContent { get; }
    string Subject { get; }
}
