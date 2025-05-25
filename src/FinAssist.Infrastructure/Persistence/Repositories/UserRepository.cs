using Dapper;
using FinAssist.Domain.Entities;
using FinAssist.Domain.Repositories;
using FinAssist.Infrastructure.Persistence.Context;
using FinAssist.Infrastructure.Persistence.Utils;

namespace FinAssist.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<UserEntity>, IUserRepository
{
    public UserRepository(DapperContext context) : base(context)
    {
    }

    public async Task<UserEntity?> GetByEmail(string email)
    {
        using var connection = Context.CreateConnection();

        var query = Q
            .Select()
            .Where(x => x.Email == email);
        
        return await GetOne(query);
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        return false;
        /*using var connection = Context.CreateConnection();
        var emailColumn = DatabaseTableHelper.GetColumnName<UserEntity>(x => x.Email);
        var query = $"SELECT COUNT(1) FROM {TableName} WHERE {emailColumn} = @Email";
        var count = await connection.ExecuteScalarAsync<int>(query, new { Email = email });
        return count > 0;*/
    }
}