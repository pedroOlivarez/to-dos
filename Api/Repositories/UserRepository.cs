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
            refresh_token,
            refresh_token_expires_at,
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
        List<string> updatedValues = ["updated_at = @now"];

        if (userUpdateDto.Password is not null)
        {
            updatedValues.Add("password = @password");
        }
        if (userUpdateDto.RefreshToken is not null)
        {
            updatedValues.Add(
                $"refresh_token = {(!string.IsNullOrWhiteSpace(userUpdateDto.RefreshToken) ? "@refreshToken" : "null")}"
            );
            updatedValues.Add(
                $"refresh_token_expires_at = {(userUpdateDto.RefreshTokenExpiresAt.HasValue ? "@refreshTokenExpiresAt" : "null")}"
            );
        }

        await Update(
            tableName,
            updatedValues,
            new
            {
                id,
                password = userUpdateDto.Password,
                now = DateTime.UtcNow,
                refreshToken = userUpdateDto.RefreshToken,
                refreshTokenExpiresAt = userUpdateDto.RefreshTokenExpiresAt,
            }
        );
    }

    // not currently in use
    public async Task Archive(int id)
    {
        await Archive(tableName, id);
    }
}
