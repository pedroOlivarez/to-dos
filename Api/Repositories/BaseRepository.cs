using Api.Contracts;
using Dapper;
using Npgsql;

namespace Api.Repositories;

public class BaseRepository(
   string connectionString
) : IBaseRepository
{
   private readonly string _connectionString = connectionString;

   public async Task<IEnumerable<T>> GetMany<T>(string sql)
   {
      using var connection = new NpgsqlConnection(_connectionString);
      return connection.Query<T>(sql);
   }
}