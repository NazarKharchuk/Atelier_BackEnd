using System.ComponentModel.DataAnnotations;

namespace Atelier.DAL.Enums
{
    public enum Status
    {
        [Display(Name = "Нове")]
        New = 0,
        [Display(Name = "Виконується")]
        InProgress = 1,
        [Display(Name = "Завершено")]
        Completed = 2,
    }
}
