namespace FinAssist.Domain.Services.Identity;

public interface ICurrentUserService
{
    CurrentUserModel User { get; }
    void Set(CurrentUserModel user);
}