namespace Notification.Domain;

using Hive.SeedWorks.TacticalPatterns;

public interface INotification : IBoundedContext { }

public class NotificationBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "Notification";
    public int MicroserviceVersion => 1;
}
