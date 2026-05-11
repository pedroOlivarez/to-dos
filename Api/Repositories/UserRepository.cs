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

   public async Task<User> GetById(int id)
   {
      return await base.GetById<User>(queryOneString, id);
   }

   public async Task<IEnumerable<User>> GetMany()
   {
      throw new NotImplementedException();
   }

   public async Task Update(UserUpdateDto updateUserDto)
   {
      List<string> updatedValues = [];

      if (updateUserDto.FirstName is not null)
      {
         updatedValues.Add("first_name = @firstName");
      }
      if (updateUserDto.LastName is not null)
      {
         updatedValues.Add("last_name = @lastName");
      }
      if (updateUserDto.Email is not null)
      {
         updatedValues.Add("email = @email");
      }

      var updateSql = @$"
         UPDATE public.users
         SET {string.Join(",", updatedValues)}
         WHERE Id = @Id
      ";

      await base.Update(updateSql, updateUserDto);
   }
}