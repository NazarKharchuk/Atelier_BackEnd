using System.ComponentModel.DataAnnotations;

namespace Atelier.PL.Models
{
    public class WorksTypeModel
    {
        public int WorksTypeId { get; set; }

        [Required(ErrorMessage = "Please enter WorksType Name.")]
        [MaxLength(30, ErrorMessage = "The length of the WorksType Name must be less than 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter WorksType Cost.")]
        [Range(0, double.MaxValue, ErrorMessage = "Error WorksType Cost range.")]
        public decimal Cost { get; set; }
    }
}
