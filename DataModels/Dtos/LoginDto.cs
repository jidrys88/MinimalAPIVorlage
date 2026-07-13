using System.ComponentModel.DataAnnotations;

namespace DataModels.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username ist erforderlich")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password ist erforderlich")]
        public string Password { get; set; } = string.Empty;
    }
}
