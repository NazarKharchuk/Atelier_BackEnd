namespace Atelier.DAL.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
