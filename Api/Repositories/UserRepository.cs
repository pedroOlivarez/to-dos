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
            refresh_token as refreshToken,
            refresh_token_expires_at as refreshTokenExpiresAt,
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

        return await GetById<User>(queryOneString, createdId)
            ?? throw new KeyNotFoundException($"User with id {createdId} not found");
    }

    public async Task<User> GetById(int id)
    {
        return await GetById<User>(queryOneString, id)
            ?? throw new KeyNotFoundException($"User with id {id} not found");
    }

    public async Task<User?> GetByEmail(string email)
    {
        var sql =
            @$"
               {baseQueryString}
               AND Email = @email
            ";
        return await GetByParams<User>(sql, new { email });
    }

    public async Task<User> GetByRefreshToken(string refreshToken)
    {
        var sql =
            @$"
            {baseQueryString}
            AND refresh_token = @refreshToken
        ";
        return await GetByParams<User>(sql, new { refreshToken })
            ?? throw new KeyNotFoundException(
                $"User with a refreshToken matching {refreshToken} not found"
            );
    }

    public async Task Update(int id, UserUpdateDto userUpdateDto)
    {
        List<string> updatedValues = ["updated_at = @now"];

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
                now = DateTime.UtcNow,
                refreshToken = userUpdateDto.RefreshToken,
                refreshTokenExpiresAt = userUpdateDto.RefreshTokenExpiresAt,
            }
        );
    }

    public async Task<bool> RefreshTokenExists(string refreshToken)
    {
        var sql =
            @$"
        SELECT EXISTS(
        SELECT 1
        FROM {tableName}
        WHERE archived = false
        AND refresh_token = @refreshToken
        )
    ";
        return await GetByParams<bool>(sql, new { refreshToken });
    }
}
