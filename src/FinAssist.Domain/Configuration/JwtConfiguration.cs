namespace FinAssist.Domain.Configuration;

public class JwtConfiguration
{
    public string SecurityKey { get; init; }
    public double SessionLifeTime { get; init; }
    public double RefreshTokenLifeTime { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
}