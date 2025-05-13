using AutoMapper;
using FinAssist.Domain;
using FinAssist.Domain.Entities;
using FinAssist.Domain.Notification;
using FinAssist.Domain.Notification.Providers;
using FinAssist.Domain.Repositories;
using FinAssist.Domain.Services.Identity;
using FinAssist.Domain.Services.Notification;
using FinAssist.Infrastructure.Exceptions;

namespace FinAssist.Infrastructure.Notification;

public class NotificationService : INotificationService
{
    private readonly INotificationProviderFactory _notificationProviderFactory;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationTemplateService _notificationTemplateService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public NotificationService(
        INotificationProviderFactory notificationProviderFactory, 
        ICurrentUserService currentUserService, 
        INotificationTemplateService notificationTemplateService, 
        IUserRepository userRepository, 
        IMapper mapper)
    {
        _notificationProviderFactory = notificationProviderFactory;
        _currentUserService = currentUserService;
        _notificationTemplateService = notificationTemplateService;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task Send(NotificationTemplateType templateType, Dictionary<string, string> properties)
    {
        var user = _mapper.Map<CurrentUserModel, NotificationContextUserInfo>(_currentUserService.User);
        
        await InternalSend(user, templateType, properties);
    }

    public async Task Send(Guid userId, NotificationTemplateType templateType, Dictionary<string, string> properties)
    {
        var userEntity = await _userRepository.GetById(userId);
        
        if (userEntity is null)
        {
            throw NotFoundException.With<UserEntity>(userId);
        }
        
        var user = _mapper.Map<UserEntity, NotificationContextUserInfo>(userEntity);
        
        await InternalSend(user, templateType, properties);
    }
    
    private async Task InternalSend(NotificationContextUserInfo user, NotificationTemplateType templateType, Dictionary<string, string> properties)
    {
        var provider = _notificationProviderFactory.Get(user.NotificationProvider);

        NotificationTemplates.Templates.TryGetValue(templateType, out var template);
        
        if (template is null)
        {
            throw new ArgumentException($"Шаблон уведомления не найден. Тип: {templateType}");
        }
        
        var context = new NotificationContext
        {
            Subject = template.Subject,
            BodyHtml = _notificationTemplateService.Render(template.BodyHtml, properties),
            BodyText = _notificationTemplateService.Render(template.BodyText, properties),
            User = user
        };

        await provider.Send(context);
    }
}