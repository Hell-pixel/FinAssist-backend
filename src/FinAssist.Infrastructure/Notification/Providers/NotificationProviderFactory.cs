using FinAssist.Domain.Enums;
using FinAssist.Domain.Notification.Providers;
using FinAssist.Domain.Services.Identity;
using FinAssist.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinAssist.Infrastructure.Notification.Providers;

public class NotificationProviderFactory : INotificationProviderFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICurrentUserService _currentUserService;

    public NotificationProviderFactory(
        IServiceProvider serviceProvider,
        ICurrentUserService currentUserService)
    {
        _serviceProvider = serviceProvider;
        _currentUserService = currentUserService;
    }

    public INotificationProvider Get()
    {
        return Get(_currentUserService.User.NotificationProvider);
    }

    public INotificationProvider Get(NotificationProviderType providerType)
    {
        if (AppConfiguration.NotificationConfiguration.ForceNullProvider)
        {
            return _serviceProvider.GetRequiredService<NullNotificationProvider>();
        }
        
        return providerType switch
        {
            NotificationProviderType.Email => _serviceProvider.GetRequiredService<SmtpNotificationProvider>(),
            _ => _serviceProvider.GetRequiredService<NullNotificationProvider>()
        };
    }
}