using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class ShopProfile
{
    public ShopProfile()
    {
        Products = new List<Product>();
        ShopComments = new List<ShopComment>();
        Orders = new List<Order>();
    }
    public int Id { get; set; }
    [Required]
    [MinLength(2)]
    [Display(Name = "Назва")]
    public string Name { get; set; }
    public int SalesmanId { get; set; }
    [Display(Name = "Верифікація")]
    public bool IsVerified { get; set; }
    [Required]
    [Display(Name = "Місто")]
    public string City { get; set; }
    [Display(Name = "Адреса")]
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    [Display(Name = "Логотип")]
    public byte[] LogoPhoto { get; set; }
    [Display(Name = "Опис")]
    public string Description { get; set; }
    [Phone]
    [Display(Name = "Номер телефону")]
    public string Contacts { get; set; }
    
    [Display(Name = "Продавець")]
    public User Salesman { get; set; }
    [Display(Name = "Товари")]
    public List<Product> Products { get; set; }
    [Display(Name = "Коментарі")]
    public List<ShopComment> ShopComments { get; set; }
    [Display(Name = "Замовлення")]
    public List<Order> Orders { get; set; }
}