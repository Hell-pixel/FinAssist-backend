using Dapper;
using FinAssist.Domain.Entities;
using FinAssist.Domain.Repositories;
using FinAssist.Infrastructure.Persistence.Context;
using FinAssist.Infrastructure.Persistence.Utils;
using static FinAssist.Infrastructure.Common.StringFormatter;

namespace FinAssist.Infrastructure.Persistence.Repositories;

public class UserSessionRepository : Repository<UserSessionEntity>, IUserSessionRepository
{
    public UserSessionRepository(DapperContext context) : base(context) { }
    
    public async Task<UserSessionEntity?> GetByIdWithUserInfo(Guid sessionId)
    {
        using var connection = Context.CreateConnection();

        var sql = @$"
        SELECT s.*, u.*
        FROM {TableName} s
        INNER JOIN {DatabaseTableHelper.GetTableName<UserEntity>()} u ON s.{GetEscapedString("UserId")} = u.{GetEscapedString("Id")}
        WHERE s.{GetEscapedString("Id")} = @SessionId";

        var result = await connection.QueryAsync<UserSessionEntity, UserEntity, UserSessionEntity>(
            sql,
            (session, user) =>
            {
                session.User = user;
                return session;
            },
            new { SessionId = sessionId },
            splitOn: "Id"
        );

        return result.FirstOrDefault();
    }
    
    public async Task<IEnumerable<UserSessionEntity>> GetByUserId(Guid userId)
    {
        using var connection = Context.CreateConnection();
        var query = $@"
        SELECT * 
        FROM {TableName} 
        WHERE {GetEscapedString("UserId")} = @UserId
        ORDER BY {GetEscapedString("CreatedAt")} DESC";
    
        return await connection.QueryAsync<UserSessionEntity>(query, new
        {
            UserId = userId
        });
    }
    
    public async Task<UserSessionEntity?> GetByRefreshTokenHash(string refreshTokenHash)
    {
        using var connection = Context.CreateConnection();
        var query = $@"
        SELECT * 
        FROM {TableName} 
        WHERE {GetEscapedString("RefreshTokenHash")} = @RefreshTokenHash
        ";
    
        return await connection.QuerySingleOrDefaultAsync<UserSessionEntity>(query, new
        {
            RefreshTokenHash = refreshTokenHash
        });
    }
    
    public async Task DeleteAllByUserId(Guid userId)
    {
        var connection = Context.CreateConnection();
        var query = $"DELETE FROM {TableName} WHERE {GetEscapedString("UserId")} = @UserId";
        await connection.ExecuteAsync(query, new { UserId = userId });
    }
}
