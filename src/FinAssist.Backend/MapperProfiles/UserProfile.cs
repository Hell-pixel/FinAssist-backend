using AutoMapper;
using FinAssist.Domain;
using FinAssist.Domain.Dtos.Account;
using FinAssist.Domain.Entities;
using FinAssist.Domain.Notification;

namespace FinAssist.Backend.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, UserEntity>();
        CreateMap<UserSessionEntity, UserSessionDto>();
        CreateMap<UserEntity, NotificationContextUserInfo>();
        CreateMap<CurrentUserModel, NotificationContextUserInfo>();
    }
}