using FinAssist.Domain.Services.Notification;

namespace FinAssist.Infrastructure.Notification;

public class NotificationTemplateService : INotificationTemplateService
{
    public string Render(string template, Dictionary<string, string> properties)
    {
        var result = template;
        
        foreach (var property in properties)
        {
            var placeholder = $"{{{{{property.Key}}}}}";
            result = result.Replace(placeholder, property.Value);
        }
        
        return result;
    }
}