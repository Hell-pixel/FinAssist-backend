
using FinAssist.Domain.Notification;

namespace FinAssist.Domain.Services.Notification;

public interface INotificationService
{
    Task Send(NotificationTemplateType templateType, Dictionary<string, string> properties);
    Task Send(Guid userId, NotificationTemplateType templateType, Dictionary<string, string> properties);
}