using Api.Dtos;
using Api.Entities;

namespace Api.Contracts;

public interface IUserRepository
{
    public Task<User> GetById(int id);

    public Task<User?> GetByEmail(string email);

    public Task<User> GetByRefreshToken(string refreshToken);

    public Task<bool> RefreshTokenExists(string refreshToken);

    public Task<User> Create(UserInsertDto userInsertDto);

    public Task Update(int id, UserUpdateDto userUpdateDto);

    public Task Archive(int id);
}
