using FinAssist.Domain;
using FinAssist.Domain.Services.Identity;

namespace FinAssist.Infrastructure.Services.Identity;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserModel User { get; private set; }
    
    public void Set(CurrentUserModel user)
    {
        User = user;
    }
}