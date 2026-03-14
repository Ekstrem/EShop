namespace Notification.Application.Queries;

using MediatR;
using Notification.InternalContracts;

public sealed class GetNotificationHandler : IRequestHandler<GetNotificationQuery, NotificationReadModel?>
{
    private readonly INotificationQueryRepository _repository;

    public GetNotificationHandler(INotificationQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<NotificationReadModel?> Handle(
        GetNotificationQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.NotificationId, cancellationToken);
    }
}
