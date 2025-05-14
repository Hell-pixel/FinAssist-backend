namespace FinAssist.Domain.Configuration;

public class SmtpConfiguration
{
    public required string Host { get; init; }
    public int Port { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required string From { get; init; }
    public bool EnableSsl { get; init; }
}