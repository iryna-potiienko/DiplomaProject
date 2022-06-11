using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Невірно вказаний email")]
    //[Display(Name = "")]
    public string Email { get; set; }
 
    [Display(Name = "Пароль")]
    [Required(ErrorMessage = "Невірно вказаний пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}