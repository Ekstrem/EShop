namespace Notification.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Notification.Domain;
using Notification.Domain.Abstraction;

public sealed record RetryNotificationCommand(Guid NotificationId)
    : IRequest<AggregateResult<INotification, INotificationAnemicModel>>;
