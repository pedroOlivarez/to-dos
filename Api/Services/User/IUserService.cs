using Api.Dtos;
using Api.Models.User;

namespace Api.Services.User;

public interface IUserService
{
    Task<UserModel> GetById(int id);

    Task<UserModel> Create(UserInsertDto userInsertDto, HttpContext context);

    Task<UserModel> Update(int id, UserUpdateDto userUpdateDto);

    Task Archive(int id);
}
