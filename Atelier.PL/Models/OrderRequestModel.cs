using System.ComponentModel.DataAnnotations;

namespace Atelier.PL.Models
{
    public class OrderRequestModel
    {
        [Required(ErrorMessage = "Please enter response Order.")]
        public OrderModel res_order { get; set; }

        public List<OrderRequestMaterialModel> res_materials { get; set; }
    }
}
