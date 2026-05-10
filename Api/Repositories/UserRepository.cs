using Api.Contracts;
using Api.Dtos;
using Api.Entities;
using Api.Settings;
using Microsoft.Extensions.Options;

namespace Api.Repositories;

public class UserRepository(
   IOptions<RepositorySettings> options
) : BaseRepository(options), IUserRepository
{
   private static readonly string baseQueryString = @"
      SELECT
         id as Id,
         first_name as FirstName,
         last_name as LastName,
         email as Email
      FROM public.users
   ";

   private static readonly string queryOneString = @$"
      {baseQueryString}
      WHERE ID = @Id
   ";

   public async Task<User> Create(UserInsertDto insertUserDto)
   {
      var insertSql = @"
         INSERT INTO public.users
         (first_name, last_name, email)
         VALUES(@firstName, @lastName, @email)
         RETURNING id;
      ";

      var createdId = await base.Create(insertSql, new
      {
         firstName = insertUserDto.FirstName,
         lastName = insertUserDto.LastName,
         email = insertUserDto.Email
      });

      return await base.GetById<User>(queryOneString, createdId);
   }

   public Task<User> GetById(int Id)
   {
      throw new NotImplementedException();
   }

   public async Task<IEnumerable<User>> GetMany()
   {
      return await base.GetMany<User>(baseQueryString);
   }

   public Task Update(UserUpdateDto updateUserDto)
   {
      throw new NotImplementedException();
   }
}