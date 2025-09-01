namespace ResturangtApi_samiharun_net24.Models.Security
{
    public class JwtSettingr
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiresMinutes { get; set; } = 60;
    }
}
