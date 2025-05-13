using System.Text.Json.Serialization;

namespace FinAssist.Domain.Notification.Providers;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationProviderType
{
    Email,
}