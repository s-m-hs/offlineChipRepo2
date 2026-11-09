namespace CY_WebApi.Models
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string RefreshTokenExpirationDays { get; set; }

        public int TokenExpiryInMinutes { get; set; }
    }
}
