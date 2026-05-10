using Api.Dtos;
using Api.Models;

namespace Api.Services.Users;

public interface IUserService
{
   Task<IEnumerable<UserModel>> GetMany();

   Task<UserModel> Create(UserInsertDto userInsertDto);
}