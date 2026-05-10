using Api.Contracts;
using Api.Settings;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Api.Repositories;

public class BaseRepository(IOptions<RepositorySettings> options) : IBaseRepository
{
   private readonly string _connectionString = options.Value.ConnectionString;

   public async Task<IEnumerable<T>> GetMany<T>(string sql)
   {
      using var connection = new NpgsqlConnection(_connectionString);
      return connection.Query<T>(sql);
   }
}