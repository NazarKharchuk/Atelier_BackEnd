using Atelier.DAL.Enums;

namespace Atelier.PL.Models
{
    public class AuthorizationResponseModel
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public Role Role { get; set; }
        public string Name { get; set; }
    }
}
