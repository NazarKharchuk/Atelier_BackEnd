using Atelier.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Atelier.PL.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Please enter Receiving Date.")]
        public DateTime ReceivingDate { get; set; }

        [Required(ErrorMessage = "Please enter Issue Date.")]
        public DateTime IssueDate { get; set; }

        [Required(ErrorMessage = "Please enter Status.")]
        public Status Status { get; set; }

        [Required(ErrorMessage = "Please enter WorksType id.")]
        public int WorkTypeId { get; set; }

        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please enter Client id.")]
        public int ClientId { get; set; }
    }
}
