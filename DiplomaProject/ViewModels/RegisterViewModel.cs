using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Не вказане Ім'я")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Не вказаний Email")]
    public string Email { get; set; }
 
    [Required(ErrorMessage = "Не вказаний пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
 
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Паролі не співпадають")]
    public string ConfirmPassword { get; set; }
}