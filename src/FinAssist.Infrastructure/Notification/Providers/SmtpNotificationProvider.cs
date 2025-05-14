using System.Net;
using System.Net.Mail;
using FinAssist.Domain.Notification;
using FinAssist.Domain.Notification.Providers;
using FinAssist.Infrastructure.Configuration;

namespace FinAssist.Infrastructure.Notification.Providers;

public class SmtpNotificationProvider : INotificationProvider
{
    public Task Send(NotificationContext context)
    {
        using var client = CreateSmtpClient();
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(AppConfiguration.NotificationConfiguration.Smtp.From),
            Subject = context.Subject,
            Body = context.BodyHtml,
            IsBodyHtml = true,
            To = { context.User.Email }
        };
        
        client.Send(mailMessage);
        return Task.CompletedTask;
    }
    
    private static SmtpClient CreateSmtpClient()
    {
        var smtpConfig = AppConfiguration.NotificationConfiguration.Smtp;
        var client = new SmtpClient(smtpConfig.Host, smtpConfig.Port)
        {
            Credentials = new NetworkCredential(smtpConfig.UserName, smtpConfig.Password),
            EnableSsl = AppConfiguration.NotificationConfiguration.Smtp.EnableSsl,
            Timeout = 5000
        };
        return client;
    }
}