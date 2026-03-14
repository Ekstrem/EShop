namespace Notification.InternalContracts;

public interface INotificationQueryRepository
{
    Task<NotificationReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NotificationReadModel>> SearchAsync(
        Guid? customerId,
        string? channel,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(
        Guid? customerId,
        string? channel,
        string? status,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByEventAndCustomerAsync(
        Guid eventId,
        Guid customerId,
        CancellationToken cancellationToken = default);
}
