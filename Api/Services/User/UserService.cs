using Api.Contracts;
using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.User;

public class UserService(
    IPasswordHasher<BaseUserDto> passwordHasher,
    IUserRepository userRepository
) : IUserService
{
    private readonly IPasswordHasher<BaseUserDto> _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;

    public Task Archive(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<PasswordVerificationResult> Authenticate(
        AuthenticationRequest authenticationRequest
    )
    {
        try
        {
            var user = await _userRepository.GetByEmail(authenticationRequest.Email);
            return _passwordHasher.VerifyHashedPassword(
                authenticationRequest,
                user.Password,
                authenticationRequest.Password
            );
        }
        catch
        {
            // don't want to return 404 here. Security concern. Just do 401 or 403 whichever one is bad auth
            return PasswordVerificationResult.Failed;
        }
    }

    public async Task<UserModel> Create(UserInsertDto userInsertDto)
    {
        // verification on email format
        // verification on email uniqueness
        // verification on password length
        var hashedPassword = _passwordHasher.HashPassword(userInsertDto, userInsertDto.Password);

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
