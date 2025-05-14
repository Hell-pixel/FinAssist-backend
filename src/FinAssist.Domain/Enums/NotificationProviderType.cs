using System.Text.Json.Serialization;

namespace FinAssist.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationProviderType
{
    Email,
}