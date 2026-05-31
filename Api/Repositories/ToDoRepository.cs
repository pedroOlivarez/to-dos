using Api.Contracts;
using Api.Dtos;
using Api.Entities;
using Api.Models.Shared.Requests;

namespace Api.Repositories;

public class ToDoRepository(IConfiguration configuration)
    : BaseRepository(configuration),
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
            user_id as userId,
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

    public async Task<ToDo> Create(ToDoInsertDto toDoInsertDto, int userId)
    {
        var insertSql =
            @$"
            INSERT INTO {tableName}
            (title, body, user_id, created_at, updated_at)
            VALUES(@title, @body, @userId, @now, @now)
            RETURNING id;
        ";

        var createdId = await Create(
            insertSql,
            new
            {
                title = toDoInsertDto.Title,
                body = toDoInsertDto.Body,
                now = DateTime.UtcNow,
                userId,
            }
        );

        return await GetById<ToDo>(queryOneString, createdId)
            ?? throw new KeyNotFoundException($"ToDo with id: {createdId} not found");
    }

    public async Task<ToDo> GetById(int toDoId)
    {
        return await GetById<ToDo>(queryOneString, toDoId)
            ?? throw new KeyNotFoundException($"ToDo with id: {toDoId} not found");
    }

    public async Task<(int Total, IEnumerable<ToDo>)> GetMany(PaginatedRequest request, int userId)
    {
        var queryClause = !string.IsNullOrWhiteSpace(request.SanitizedQuery)
            ? @$"
                AND (
                    LOWER(title) LIKE @query
                    OR LOWER(body) LIKE @query
                ) 
            "
            : null;
        var paginatedSql =
            @$"
            {baseQueryString}
            AND user_id = @userId
            {queryClause}
            ORDER BY completed ASC, updated_at DESC
            LIMIT {request.PageSize}
            OFFSET {request.OffSet}
        ";
        var countSql =
            @$"
            SELECT COUNT(id)
            FROM {tableName}
            WHERE archived = false
            AND user_id = @userId
            {queryClause}
        ";
        var sqlParams = new { userId, query = request.SanitizedQuery };
        var total = await GetCount(countSql, sqlParams);
        var data = total != 0 ? await GetMany<ToDo>(paginatedSql, sqlParams) : [];

        return (total, data);
    }

    public async Task Update(int id, ToDoUpdateDto toDoUpdateDto)
    {
        List<string> updatedValues = ["updated_at = @now"];

        if (toDoUpdateDto.Title is not null)
        {
            updatedValues.Add("title = @title");
        }
        if (toDoUpdateDto.Body is not null)
        {
            updatedValues.Add(
                $"body = {(string.IsNullOrWhiteSpace(toDoUpdateDto.Body) ? "null" : "@body")}"
            );
        }
        if (toDoUpdateDto.Completed is not null)
        {
            updatedValues.Add("completed = @completed");
        }

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
