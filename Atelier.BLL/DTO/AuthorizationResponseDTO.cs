using Atelier.DAL.Enums;

namespace Atelier.BLL.DTO
{
    public class AuthorizationResponseDTO
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public Role Role { get; set; }
        public string Name { get; set; }
    }
}
