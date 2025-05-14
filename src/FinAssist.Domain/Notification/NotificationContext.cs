using FinAssist.Domain.Enums;
using FinAssist.Domain.Notification.Providers;

namespace FinAssist.Domain.Notification;

public class NotificationContext
{
    public required NotificationContextUserInfo User { get; set; }
    public string Subject { get; set; } = null!;
    public string BodyHtml { get; set; } = null!;
    public string BodyText { get; set; } = null!;
}


public record NotificationContextUserInfo(Guid Id, string Email, string Name, NotificationProviderType NotificationProvider);