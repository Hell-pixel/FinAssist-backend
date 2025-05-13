using FinAssist.Domain.Configuration;
using Microsoft.Extensions.Configuration;

namespace FinAssist.Infrastructure.Configuration;

public static class AppConfiguration
{
    public static void Init(ConfigurationManager builderConfiguration)
    {
        JwtConfiguration = builderConfiguration.GetSection("JWT").Get<JwtConfiguration>()!;
        NotificationConfiguration = builderConfiguration.GetSection("Notification").Get<NotificationConfiguration>()!;
    }
    
    public static JwtConfiguration JwtConfiguration { get; private set; } = null!;
    public static NotificationConfiguration NotificationConfiguration { get; private set; } = null!;
}