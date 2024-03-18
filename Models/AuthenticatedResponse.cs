using System.Security.Claims;

namespace JwtAuthenticationWebAPI.Models
{
    public class AuthenticatedResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }

        public List<string> Claims { get; set; }
    }
}
