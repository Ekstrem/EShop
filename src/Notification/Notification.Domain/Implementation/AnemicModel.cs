namespace Notification.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Notification.Domain.Abstraction;

internal sealed class AnemicModel : AnemicModel<INotification>, INotificationAnemicModel
{
    public INotificationRoot Root { get; internal set; } = null!;
    public string RenderedContent { get; internal set; } = string.Empty;
    public string Subject { get; internal set; } = string.Empty;
}
