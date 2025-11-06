namespace Domain.Configs;

public class JwtSettingsConfig
{
    public const string Key = "JwtSettings";
    
    public string JwtSecurityKey { get; set; }
    public string JwtIssuer { get; set; }
    public string JwtAudience { get; set; }
    public int JwtExpiryInDays { get; set; }
}