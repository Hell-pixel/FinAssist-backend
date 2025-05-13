namespace FinAssist.Domain.Notification.Providers;

public interface INotificationProvider
{
    Task Send(NotificationContext context);
}