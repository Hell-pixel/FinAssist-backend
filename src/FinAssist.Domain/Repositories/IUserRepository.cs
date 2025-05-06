using FinAssist.Domain.Entities;

namespace FinAssist.Domain.Repositories;

public interface IUserRepository : IRepository<UserEntity>
{
    public Task<UserEntity?> GetByEmail(string email);
    Task<bool> ExistsByEmail(string email);
}
