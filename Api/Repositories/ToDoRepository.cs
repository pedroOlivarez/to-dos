using Api.Contracts;
using Api.Dtos;
using Api.Entities;
using Api.Models.Requests;
using Api.Settings;
using Microsoft.Extensions.Options;

namespace Api.Repositories;

public class ToDoRepository(
    IOptions<RepositorySettings> options
) : BaseRepository(options), IToDoRepository
{
    private static readonly string tableName = "public.to_dos";

    private static readonly string baseQueryString =@$"
        SELECT
            id,
            title,
            body,
            created_at as createdAt,
            updated_at as updatedAt
        FROM {tableName}
        WHERE archived = false
    ";

    private static readonly string queryOneString = @$"
        {baseQueryString}
        AND Id = @Id
    ";

    public async Task<ToDo> Create(ToDoInsertDto toDoInsertDto)
    {
        var insertSql = @$"
            INSERT INTO {tableName}
            (title, body, created_at, updated_at)
            VALUES(@title, @body, @now, @now)
            RETURNING id;
        ";

        var createdId = await base.Create(insertSql, new
        {
            title = toDoInsertDto.Title,
            body = toDoInsertDto.Body,
            now = DateTime.UtcNow
        });

        return await base.GetById<ToDo>(queryOneString, createdId);
    }

    public Task<ToDo> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<(int Total, IEnumerable<ToDo>)> GetMany(PaginatedRequest request)
    {
        var paginatedSql = @$"
        {baseQueryString}
        LIMIT {request.PageSize}
        OFFSET {request.OffSet}
        ";
        return await base.GetMany<ToDo>(paginatedSql, tableName);
    }

    public Task Update(int id, ToDoUpdateDto toDoUpdateDto)
    {
        throw new NotImplementedException();
    }
}