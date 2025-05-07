using AutoMapper;
using FinAssist.Domain.Dtos.Account;
using FinAssist.Domain.Entities;

namespace FinAssist.Backend.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, UserEntity>();
        CreateMap<UserSessionEntity, UserSessionDto>();
    }
}