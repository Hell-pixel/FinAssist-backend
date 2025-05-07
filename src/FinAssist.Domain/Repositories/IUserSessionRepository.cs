using FinAssist.Domain.Entities;

namespace FinAssist.Domain.Repositories;

public interface IUserSessionRepository : IRepository<UserSessionEntity>
{
    Task<UserSessionEntity?> GetByIdWithUserInfo(Guid sessionId);
    Task<IEnumerable<UserSessionEntity>> GetByUserId(Guid userId);
    Task<UserSessionEntity?> GetByRefreshTokenHash(string refreshTokenHash);
    Task DeleteAllByUserId(Guid userId);
}
