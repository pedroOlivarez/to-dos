using Api.Models;

namespace Api.Services.Users;

public interface IUserService
{
   Task<IEnumerable<UserModel>> GetMany();
}