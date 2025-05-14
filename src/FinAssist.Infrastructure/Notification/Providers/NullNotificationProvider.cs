using FinAssist.Domain.Notification;
using FinAssist.Domain.Notification.Providers;

namespace FinAssist.Infrastructure.Notification.Providers;

public class NullNotificationProvider : INotificationProvider
{
    public Task Send(NotificationContext context)
    {
        Console.WriteLine($"[NullProvider] Уведомление для пользователя {context.User.Id} ({context.User.Name}). \n {context.BodyText}");
        return Task.CompletedTask;
    }
}