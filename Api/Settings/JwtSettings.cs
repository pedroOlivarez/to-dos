namespace Api.Settings;

public class JwtSettings
{
    public const string JwtOptionsKey = "JwtOptions";

    public required string Secret { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int ExpirationTimeInMinutes { get; set; }
}
