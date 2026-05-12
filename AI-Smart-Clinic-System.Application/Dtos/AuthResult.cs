using System.Text.Json.Serialization;

namespace AI_Smart_Clinic_System.Application.Dtos
{
    public class AuthResult
    {
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Token { get; set; }
        public bool IsAuthenticated{ get; set; }
        // public string Role { get; set; } = null!;
        public DateTime ExpiresOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresOn { get; set; }

    }
}
