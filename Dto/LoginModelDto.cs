using System.ComponentModel.DataAnnotations;

namespace JWTAuthenticationWebAPI.Dto
{
    public class LoginModelDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
