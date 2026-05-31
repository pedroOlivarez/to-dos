using Api.Dtos;
using Api.Models.User;

namespace Api.Services.User;

public interface IUserService
{
    Task<bool> CheckIfEmailInUse(string email);

    Task<UserModel> Create(UserInsertDto userInsertDto, HttpContext context);
}
