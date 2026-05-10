using Api.Dtos;
using Api.Entities;

namespace Api.Contracts;

public interface IUserRepository
{
   public Task<IEnumerable<User>> GetMany();

   public Task<User> GetById(int Id);

   public Task<User> Create(UserInsertDto insertUserDto);

   public Task Update(UserUpdateDto updateUserDto);
}