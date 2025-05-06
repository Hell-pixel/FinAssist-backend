using FinAssist.Domain.Dtos.Account;

namespace FinAssist.Domain.Services.Identity;

public interface IAccountService
{
    Task<AccountTokenDto> Login(LoginUserDto userDto, string userAgent, string? realIp);
    Task ChangePassword(UserPasswordChangeDto userPasswordChangeDto);
    Task Register(CreateUserDto createUserDto);
    Task<CurrentUserModel> GetCurrentUserInfo();
    Task<IEnumerable<UserSessionDto>> GetSessions();
    Task<AccountTokenDto> RefreshSession(string refreshToken);
    Task Logout();
    Task LogoutAllSessions();
}