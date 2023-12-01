using System.ComponentModel.DataAnnotations;

namespace Atelier.DAL.Enums
{
    public enum Role
    {
        [Display(Name = "Співробітник")]
        User = 0,
        [Display(Name = "Адмін")]
        Admin = 1,
    }
}
