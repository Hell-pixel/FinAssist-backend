namespace FinAssist.Domain.Configuration;

public class NotificationConfiguration
{
    public bool ForceNullProvider { get; init; }
    public SmtpConfiguration Smtp { get; init; }
}