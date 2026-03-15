namespace Notification.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Notification.Domain.Abstraction;

internal sealed class AnemicModel : INotificationAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public INotificationRoot Root { get; internal set; } = null!;
    public string RenderedContent { get; internal set; } = string.Empty;
    public string Subject { get; internal set; } = string.Empty;

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
