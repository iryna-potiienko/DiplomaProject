using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class ReadyStage
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [Display(Name = "Стадія готовності")]
    public string Name { get; set; }
}