using Api.Contracts;
using Api.Models;

namespace Api.Services.Users;

public class UserService(
   IUserRepository userRepository
) : IUserService
{
   private readonly IUserRepository _userRepository = userRepository;
   public async Task<IEnumerable<UserModel>> GetMany()
   {
      var users = await _userRepository.GetMany();

      return users.Select(u => new UserModel
      {
         FirstName = u.FirstName,
         LastName = u.LastName,
         Email = u.Email
      });
   }
}