using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class Role
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [Display(Name = "Роль")]
    public string Name { get; set; }
}