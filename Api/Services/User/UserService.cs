using Api.Authentication;
using Api.Contracts;
using Api.Dtos;
using Api.Models.User;

namespace Api.Services.User;

public class UserService(IPasswordHasher passwordHasher, IUserRepository userRepository)
    : IUserService
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;

    public Task Archive(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<UserModel> Create(UserInsertDto userInsertDto)
    {
        // verification on email format
        // verification on email uniqueness
        // verification on password length
        var hashedPassword = _passwordHasher.HashPassword(userInsertDto.Password);

        userInsertDto.Password = hashedPassword;

        var user = await _userRepository.Create(userInsertDto);

        return user.ToModel();
    }

    public Task<UserModel> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> Update(int id, UserUpdateDto userUpdateDto)
    {
        throw new NotImplementedException();
    }
}
