using System.ComponentModel.DataAnnotations;

namespace Atelier.PL.Models
{
    public class MaterialModel
    {
        public int MaterialId { get; set; }

        [Required(ErrorMessage = "Please enter Material Name.")]
        [MaxLength(30, ErrorMessage = "The length of the Material Name must be less than 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Material Quantity.")]
        [Range(0, double.MaxValue, ErrorMessage = "Error Material Quantity range.")]
        public decimal Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Error Material Reserve range.")]
        public decimal Reserve { get; set; }

        [Required(ErrorMessage = "Please enter Material Cost.")]
        [Range(0, double.MaxValue, ErrorMessage = "Error Material Cost range.")]
        public decimal Cost { get; set; }
    }
}
