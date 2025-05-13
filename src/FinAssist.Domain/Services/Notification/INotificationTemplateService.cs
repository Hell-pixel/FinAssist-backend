namespace FinAssist.Domain.Services.Notification;

public interface INotificationTemplateService
{
    string Render(string template, Dictionary<string, string> properties);
}