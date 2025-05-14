using FinAssist.Domain.Enums;

namespace FinAssist.Domain.Notification.Providers;

public interface INotificationProviderFactory
{
    INotificationProvider Get();
    INotificationProvider Get(NotificationProviderType providerType);
}