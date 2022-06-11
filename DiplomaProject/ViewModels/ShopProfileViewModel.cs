using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DiplomaProject.ViewModels;

public class ShopProfileViewModel
{
    public int Id { get; set; }
    [Display(Name = "Ім'я")]
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [MinLength(3)]
    public string Name { get; set; }
    
    [Display(Name = "Місто")]
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    public string City { get; set; }
    [Display(Name = "Адреса")]
    public string Address { get; set; }
    [Display(Name = "Email власника")]
    public string Username { get; set; }
    
    [Display(Name = "Фото")]
    public IFormFile LogoPhoto { get; set; }
    [Display(Name = "Опис")]
    public string Description { get; set; }
    [Display(Name = "Номер телефону")]
    [Phone]
    public string Contacts { get; set; }
}