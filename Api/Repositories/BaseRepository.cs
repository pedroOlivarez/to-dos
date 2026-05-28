using Api.Contracts;
using Dapper;
using Npgsql;

namespace Api.Repositories;

public class BaseRepository(IConfiguration configuration) : IBaseRepository
{
    private readonly string _connectionString =
        configuration["ConnectionStrings:Neon"]
        ?? throw new ArgumentNullException(nameof(_connectionString));

    public async Task<T?> GetById<T>(string Sql, int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<T?>(Sql, new { id });
    }

    public async Task<T?> GetByParams<T>(string Sql, object sqlParams)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<T?>(Sql, sqlParams);
    }

    public async Task<(int Total, IEnumerable<T>)> GetMany<T>(string Sql, string tableName)
    {
        var countQuery =
            @$"
         SELECT COUNT(id)
         FROM {tableName}
         WHERE archived = false
      ";
        using var connection = new NpgsqlConnection(_connectionString);
        var total = await connection.QueryFirstOrDefaultAsync<int>(countQuery);
        if (total == 0)
        {
            return (total, []);
        }
        var data = await connection.QueryAsync<T>(Sql) ?? [];
        return (total, data);
    }

    public async Task<int> Create(string Sql, object values)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>(Sql, values);
    }

    public async Task Update(string tableName, IEnumerable<string> properties, object values)
    {
        var updateSql =
            @$"
         UPDATE {tableName}
         SET {string.Join(",", properties)}
         WHERE id = @id
      ";
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(updateSql, values);
    }

    public async Task Archive(string tableName, int id)
    {
        var archiveSql =
            @$"
         UPDATE {tableName}
         SET archived = true
         WHERE id = @id
      ";
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(archiveSql, new { id = id });
    }
}
