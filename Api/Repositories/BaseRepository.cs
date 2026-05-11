using Api.Contracts;
using Api.Settings;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Api.Repositories;

public class BaseRepository(IOptions<RepositorySettings> options) : IBaseRepository
{
   private readonly string _connectionString = options.Value.ConnectionString;

   public async Task<T> GetById<T>(string Sql, int Id)
   {
      using var connection = new NpgsqlConnection(_connectionString);
      return await connection.QueryFirstAsync<T>(Sql, new
      {
         Id
      });
   }

   public async Task<(int Total, IEnumerable<T>)> GetMany<T>(string Sql, string tableName)
   {
      var countQuery = @$"
         SELECT COUNT(id)
         FROM {tableName}
         WHERE archived = false
      ";
      using var connection = new NpgsqlConnection(_connectionString);
      var total = await connection.QueryFirstAsync<int>(countQuery);
      var data = await connection.QueryAsync<T>(Sql);
      return (total, data);
   }

   public async Task<int> Create(string Sql, object values)
   {
      using var connection = new NpgsqlConnection(_connectionString);
      return await connection.ExecuteScalarAsync<int>(Sql, values);
   }

   public async Task Update(string Sql, object values)
   {
      using var connection = new NpgsqlConnection(_connectionString);
      await connection.ExecuteAsync(Sql, values);
   }
}