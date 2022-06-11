using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using Microsoft.AspNetCore.Http;

namespace DiplomaProject.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }
    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [MinLength(3)]
    public string Name { get; set; }
    [Display(Name = "Магазин")]
    public int ShopProfileId { get; set; }
    [Display(Name = "Опис")]
    public string Description { get; set; }
    
    [Display(Name = "Фото")]
    public IFormFile Photo { get; set; }
    [Display(Name = "Склад продукту")]
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [MinLength(3)]
    public string Composition { get; set; }
    [Display(Name = "Ціна")]
    [Min(0,ErrorMessage = "Значення має бути більше за 0")]
    //[RegularExpression("([1-9][0-9]*)",ErrorMessage = "Введіть число більше за 0")] 
    public double Price { get; set; }
    [Display(Name = "Підкатегорія")]
    public int SubcategoryId { get; set; }
}