using FinAssist.Domain.Configuration;
using Microsoft.Extensions.Configuration;

namespace FinAssist.Infrastructure.Configuration;

public class AppConfiguration
{
    public static void Init(ConfigurationManager builderConfiguration)
    {
        JwtConfiguration = builderConfiguration.GetSection("JWT").Get<JwtConfiguration>();
    }
    
    public static JwtConfiguration JwtConfiguration { get; private set; }
}