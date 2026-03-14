namespace Notification.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Notification.Domain;
using Notification.Domain.Abstraction;

public sealed record RenderNotificationCommand(
    Guid NotificationId,
    string RenderedContent,
    string Subject)
    : IRequest<AggregateResult<INotification, INotificationAnemicModel>>;
