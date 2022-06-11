using System.ComponentModel.DataAnnotations;
using DiplomaProject.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaProject.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Ім'я")]
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [Remote(action: "VerifyUniqueEmail", controller: "Account")]
    public string Email { get; set; }
 
    [Display(Name = "Пароль")]
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Display(Name = "Місто проживання")]
    public string City { get; set; }
 
    [Display(Name = "Повторіть пароль")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Паролі не співпадають")]
    public string ConfirmPassword { get; set; }
}