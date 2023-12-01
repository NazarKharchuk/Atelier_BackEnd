using Atelier.DAL.Enums;

namespace Atelier.DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public Employee Employee { get; set; }
    }
}
