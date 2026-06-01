using Api.Authentication;
using Api.Contracts;
using Api.Dtos;
using Api.Models.User;
using Api.Services.Auth;

namespace Api.Services.User;

public class UserService(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IAuthService authService
) : IUserService
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthService _authService = authService;

    public async Task<UserModel> Create(UserInsertDto userInsertDto, HttpContext context)
    {
        var hashedPassword = _passwordHasher.HashPassword(userInsertDto.Password);

        userInsertDto.Password = hashedPassword;

        var user = await _userRepository.Create(userInsertDto);
        await _authService.SetTokens(user, context);

        return user.ToModel();
    }

    public async Task<bool> CheckIfEmailInUse(string email)
    {
        var user = await _userRepository.GetByEmail(email);
        return user != null;
    }
}
