namespace FinAssist.Domain.Configuration;

public class NotificationConfiguration
{
    /// <summary>
    /// URL-адрес приложения, используемый для генерации ссылок в уведомлениях
    /// </summary>
    public required string AppUrl { get; init; }
    public bool ForceNullProvider { get; init; }
    public SmtpConfiguration Smtp { get; init; } = null!;
}