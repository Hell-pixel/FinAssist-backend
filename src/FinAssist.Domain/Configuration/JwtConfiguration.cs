namespace FinAssist.Domain.Configuration;

public class JwtConfiguration
{
    public required string SecurityKey { get; init; }
    public double SessionLifeTime { get; init; }
    public double RefreshTokenLifeTime { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
}