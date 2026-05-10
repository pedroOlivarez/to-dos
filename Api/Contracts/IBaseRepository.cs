namespace Api.Contracts;

public interface IBaseRepository
{
   public Task<IEnumerable<T>> GetMany<T>(string sql);
}