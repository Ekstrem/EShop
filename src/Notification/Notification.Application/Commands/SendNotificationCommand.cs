namespace Notification.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Notification.Domain;
using Notification.Domain.Abstraction;

public sealed record SendNotificationCommand(
    Guid NotificationId,
    bool HasConsent)
    : IRequest<AggregateResult<INotification, INotificationAnemicModel>>;
