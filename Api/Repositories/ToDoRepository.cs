using Api.Contracts;
using Api.Dtos;
using Api.Entities;
using Api.Models.Requests;
using Api.Settings;
using Microsoft.Extensions.Options;

namespace Api.Repositories;

public class ToDoRepository(IOptions<RepositorySettings> options)
    : BaseRepository(options),
        IToDoRepository
{
    private static readonly string tableName = "public.to_dos";

    private static readonly string baseQueryString =
        @$"
        SELECT
            id,
            title,
            body,
            completed,
            created_at as createdAt,
            updated_at as updatedAt
        FROM {tableName}
        WHERE archived = false
    ";

    private static readonly string queryOneString =
        @$"
        {baseQueryString}
        AND Id = @Id
    ";

    public async Task<ToDo> Create(ToDoInsertDto toDoInsertDto)
    {
        var insertSql =
            @$"
            INSERT INTO {tableName}
            (title, body, created_at, updated_at)
            VALUES(@title, @body, @now, @now)
            RETURNING id;
        ";

        var createdId = await Create(
            insertSql,
            new
            {
                title = toDoInsertDto.Title,
                body = toDoInsertDto.Body,
                now = DateTime.UtcNow,
            }
        );

        return await GetById<ToDo>(queryOneString, createdId);
    }

    public async Task<ToDo> GetById(int id)
    {
        return await GetById<ToDo>(queryOneString, id);
    }

    public async Task<(int Total, IEnumerable<ToDo>)> GetMany(PaginatedRequest request)
    {
        var paginatedSql =
            @$"
        {baseQueryString}
        ORDER BY completed ASC, created_at
        LIMIT {request.PageSize}
        OFFSET {request.OffSet}
        ";
        return await GetMany<ToDo>(paginatedSql, tableName);
    }

    public async Task Update(int id, ToDoUpdateDto toDoUpdateDto)
    {
        List<string> updatedValues = [];

        if (toDoUpdateDto.Title is not null)
        {
            updatedValues.Add("title = @title");
        }
        if (toDoUpdateDto.Body is not null)
        {
            updatedValues.Add("body = @body");
        }
        if (toDoUpdateDto.Completed is not null)
        {
            updatedValues.Add("completed = @completed");
        }
        updatedValues.Add("updated_at = @now");

        await Update(
            tableName,
            updatedValues,
            new
            {
                id,
                title = toDoUpdateDto.Title,
                body = toDoUpdateDto.Body,
                completed = toDoUpdateDto.Completed,
                now = DateTime.UtcNow,
            }
        );
    }

    public async Task Archive(int id)
    {
        await Archive(tableName, id);
    }
}
