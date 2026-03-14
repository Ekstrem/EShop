namespace Notification.Application.Queries;

using MediatR;
using Notification.InternalContracts;

public sealed record GetNotificationQuery(Guid NotificationId)
    : IRequest<NotificationReadModel?>;
