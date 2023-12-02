using System.ComponentModel.DataAnnotations;

namespace Atelier.PL.Models
{
    public class ClientModel
    {
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Please enter Client Firstame.")]
        [MaxLength(20, ErrorMessage = "The length of the Client FirstName must be less than 20 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Client LastName.")]
        [MaxLength(20, ErrorMessage = "The length of the Client LastName must be less than 20 characters.")]
        public string LastName { get; set; }

        [MaxLength(20, ErrorMessage = "The length of the Client MiddleName must be less than 20 characters.")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Please enter Client phone number.")]
        [MaxLength(13, ErrorMessage = "The length of the Client phone number must be less than 13 characters.")]
        public string PhoneNumber { get; set; }

        [MaxLength(50, ErrorMessage = "The length of the Client Address must be less than 50 characters.")]
        public string Address { get; set; }
    }
}
