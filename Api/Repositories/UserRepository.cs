using Api.Contracts;
using Api.Entities;

namespace Api.Repositories;

public class UserRepository(
   string connectionString
) : BaseRepository(connectionString), IUserRepository
{
   private readonly string queryManyString = @"
      SELECT
         id as Id,
         first_name as FirstName,
         last_name as LastName,
         email as Email
      FROM public.users
   ";
   public async Task<IEnumerable<User>> GetMany()
   {
      return await base.GetMany<User>(queryManyString);
   }
}