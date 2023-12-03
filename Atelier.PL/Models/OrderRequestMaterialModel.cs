using System.ComponentModel.DataAnnotations;

namespace Atelier.PL.Models
{
    public class OrderRequestMaterialModel
    {
        [Required(ErrorMessage = "Please enter Material ID.")]
        public int MaterialId { get; set; }

        [Required(ErrorMessage = "Please enter material Count.")]
        [Range(0, double.MaxValue, ErrorMessage = "Error Material Count range.")]
        public decimal Count { get; set; }
    }
}
