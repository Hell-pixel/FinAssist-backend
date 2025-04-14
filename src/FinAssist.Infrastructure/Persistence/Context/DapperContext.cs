using System.Data;
using Npgsql;

namespace FinAssist.Infrastructure.Persistence.Context;

public class DapperContext(string connectionString)
{
    private readonly NpgsqlDataSource _dataSource = NpgsqlDataSource.Create(connectionString);

    public IDbConnection CreateConnection() => _dataSource.CreateConnection();
}