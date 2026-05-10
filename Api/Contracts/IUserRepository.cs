using Api.Entities;

namespace Api.Contracts;

public interface IUserRepository
{
   public Task<IEnumerable<User>> GetMany();
}