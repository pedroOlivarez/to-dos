using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.User;

public interface IUserService
{
    Task<UserModel> GetById(int id);

    Task<UserModel> Create(UserInsertDto userInsertDto);

    Task<UserModel> Update(int id, UserUpdateDto userUpdateDto);

    Task Archive(int id);

    // this should go in auth service later
    Task<PasswordVerificationResult> Authenticate(AuthenticationRequest authenticationRequest);
}

public class AuthenticationRequest : BaseUserDto
{
    public required string Email { get; set; }
}
