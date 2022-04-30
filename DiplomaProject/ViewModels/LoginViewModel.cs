using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Невірно вказаний e-mail")]
    public string Email { get; set; }
 
    [Required(ErrorMessage = "Невірно вказаний пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}