namespace Api.Contracts;

public interface IBaseRepository
{
    public Task<(int Total, IEnumerable<T>)> GetMany<T>(string Sql, string tableName);
    public Task<T?> GetById<T>(string Sql, int Id);
    public Task<T?> GetByParams<T>(string Sql, object sqlParams);
    public Task<int> Create(string Sql, object values);
    public Task Update(string tableName, IEnumerable<string> properties, object values);
    public Task Archive(string tableName, int Id);
}
