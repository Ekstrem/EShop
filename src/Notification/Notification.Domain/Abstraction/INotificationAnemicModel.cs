namespace Notification.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface INotificationAnemicModel : IAnemicModel<INotification>
{
    INotificationRoot Root { get; }
    string RenderedContent { get; }
    string Subject { get; }
}
