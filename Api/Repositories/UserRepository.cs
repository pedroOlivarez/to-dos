using Api.Contracts;
using Api.Dtos;
using Api.Entities;

namespace Api.Repositories;

public class UserRepository(IConfiguration configuration)
    : BaseRepository(configuration),
        IUserRepository
{
    private static readonly string tableName = "public.users";

    private static readonly string baseQueryString =
        @$"
        SELECT
            id,
            email,
            password,
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

    public async Task<User> Create(UserInsertDto userInsertDto)
    {
        var insertSql =
            @$"
            INSERT INTO {tableName}
            (email, password, created_at, updated_at)
            VALUES(@email, @password, @now, @now)
            RETURNING id;
        ";

        var createdId = await Create(
            insertSql,
            new
            {
                email = userInsertDto.Email,
                password = userInsertDto.Password,
                now = DateTime.UtcNow,
            }
        );

        return await GetById<User>(queryOneString, createdId);
    }

    public async Task<User> GetById(int id)
    {
        return await GetById<User>(queryOneString, id);
    }

    public async Task<User> GetByEmail(string email)
    {
        var sql =
            @$"
               {baseQueryString}
               AND Email = @email
            ";
        return await GetByParams<User>(sql, new { email });
    }

    public async Task Update(int id, UserUpdateDto userUpdateDto)
    {
        List<string> updatedValues = ["password = @password", "updated_at = @now"];

        await Update(
            tableName,
            updatedValues,
            new
            {
                id,
                password = userUpdateDto.Password,
                now = DateTime.UtcNow,
            }
        );
    }

    public async Task Archive(int id)
    {
        await Archive(tableName, id);
    }
}
