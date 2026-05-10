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

   public async Task<IEnumerable<T>> GetMany<T>(string Sql)
   {
      using var connection = new NpgsqlConnection(_connectionString);
      return await connection.QueryAsync<T>(Sql);
   }

   public async Task<int> Create(string Sql, object values)
   {
      using var connection = new NpgsqlConnection(_connectionString);
      return await connection.ExecuteScalarAsync<int>(Sql, values);
   }

   public async Task Update(string Sql, int Id)
   {
      using var connection = new NpgsqlConnection(_connectionString);
      await connection.ExecuteAsync(Sql, new
      {
         Id
      });
   }
}