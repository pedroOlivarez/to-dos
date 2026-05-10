namespace Api.Contracts;

public interface IBaseRepository
{
   public Task<IEnumerable<T>> GetMany<T>(string Sql);

   public Task<T> GetById<T>(string Sql, int Id);

   public Task<int> Create(string Sql, object values);

   public Task Update(string Sql, int Id);
}