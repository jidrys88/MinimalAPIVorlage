using System.ComponentModel.DataAnnotations;

namespace DataModels.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username ist erforderlich")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username muss zwischen 3 und 50 Zeichen lang sein")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password ist erforderlich")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password muss mindestens 6 Zeichen lang sein")]
        public string Password { get; set; } = string.Empty;
    }
}
