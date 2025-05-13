namespace FinAssist.Domain.Notification;

public enum NotificationTemplateType
{
    SuccessLogin,
    Welcome,
    PasswordReset,
    AccountBlocked
}

public class NotificationTemplate
{
    public required string Subject { get; set; }
    public required string BodyHtml { get; set; }
    public required string BodyText { get; set; }
    public NotificationTemplateType TemplateType { get; set; }
}

public static class NotificationTemplates
{
    public static readonly Dictionary<NotificationTemplateType, NotificationTemplate> Templates = new()
    {
        {
            NotificationTemplateType.SuccessLogin,
            new NotificationTemplate
            {
                Subject = "[FinAssist] Успешный вход в систему",

                BodyHtml = """
                           <table style="width: 100%; font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;">
                                   <td style="max-width: 600px; margin: auto; background-color: #fff; border-radius: 8px; padding: 30px;">
                                       <p>Вы успешно вошли в систему <strong>FinAssist</strong>.</p>

                                       <table style="margin: 20px 0; width: 100%; border-collapse: collapse;">
                                           <tr>
                                               <td style="padding: 8px 0;"><strong>📱 Устройство:</strong></td>
                                               <td style="padding: 8px 0;">{{UserAgent}}</td>
                                           </tr>
                                           <tr>
                                               <td style="padding: 8px 0;"><strong>🌐 IP-адрес:</strong></td>
                                               <td style="padding: 8px 0;">{{IPAddress}}</td>
                                           </tr>
                                           <tr>
                                               <td style="padding: 8px 0;"><strong>🕒 Дата и время:</strong></td>
                                               <td style="padding: 8px 0;">{{LoginTime}}</td>
                                           </tr>
                                       </table>

                                       <p style="color: #C0392B;"><strong>Если это были не вы</strong>, пожалуйста, 
                                       <a href="{{SessionManagementUrl}}">завершите все активные сессии</a> и смените пароль.</p>

                                       <p style="margin-top: 10px;">С уважением,<br>Команда FinAssist</p>
                                   </td>
                           </table>
                           """,

                BodyText = """
                           Вы успешно вошли в систему FinAssist.

                           🌐 IP-адрес: {{IPAddress}}
                           📱 Устройство: {{UserAgent}}
                           🕒 Дата и время: {{LoginTime}}

                           Если это были не вы, завершите все активные сессии по ссылке: {{SessionManagementUrl}} и смените пароль.
                           """,

                TemplateType = NotificationTemplateType.SuccessLogin
            }
        }
    };
}