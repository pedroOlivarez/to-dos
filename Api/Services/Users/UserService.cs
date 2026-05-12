using Api.Contracts;
using Api.Dtos;
using Api.Models;

namespace Api.Services.Users;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<UserModel>> GetMany()
    {
        var users = await _userRepository.GetMany();

        return users.Select(u => new UserModel
        {
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
        });
    }

    public async Task<UserModel> Create(UserInsertDto userInsertDto)
    {
        var user = await _userRepository.Create(userInsertDto);

        return new UserModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };
    }

    public async Task<UserModel> Update(UserUpdateDto userUpdateDto)
    {
        await _userRepository.Update(userUpdateDto);

        var user = await _userRepository.GetById(userUpdateDto.Id);

        return new UserModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };
    }
}
