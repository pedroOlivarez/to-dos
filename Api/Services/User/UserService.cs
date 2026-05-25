using Api.Contracts;
using Api.Dtos;
using Api.Models.User;

namespace Api.Services.User;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public Task Archive(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<UserModel> Create(UserInsertDto userInsertDto)
    {
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
