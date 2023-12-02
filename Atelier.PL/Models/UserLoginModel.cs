using System.ComponentModel.DataAnnotations;

namespace Atelier.PL.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Please enter Login.")]
        [MaxLength(30, ErrorMessage = "The length of the Login must be less than 30 characters.")]
        [MinLength(5, ErrorMessage = "The length of the Login must be more than 5 characters.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Please enter Password.")]
        [MinLength(5, ErrorMessage = "The length of the Password must be more than 5 characters.")]
        public string Password { get; set; }
    }
}
