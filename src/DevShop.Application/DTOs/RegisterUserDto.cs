using System.ComponentModel.DataAnnotations;

namespace DevShop.Application.DTOs
{
    public class RegisterUserDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
