using System.ComponentModel.DataAnnotations;

namespace Atelier.PL.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please enter Employee Firstame.")]
        [MaxLength(20, ErrorMessage = "The length of the Employee FirstName must be less than 20 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Employee LastName.")]
        [MaxLength(20, ErrorMessage = "The length of the Employee LastName must be less than 20 characters.")]
        public string LastName { get; set; }

        [MaxLength(20, ErrorMessage = "The length of the Employee MiddleName must be less than 20 characters.")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Please enter Employee phone number.")]
        [MaxLength(13, ErrorMessage = "The length of the Employee phone number must be less than 13 characters.")]
        public string PhoneNumber { get; set; }

        [MaxLength(50, ErrorMessage = "The length of the Employee Address must be less than 50 characters.")]
        public string Address { get; set; }

        public int UserId { get; set; }
    }
}
