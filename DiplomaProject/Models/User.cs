using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace DiplomaProject.Models;

public class User
{
    public User()
    {
        LikedProducts = new List<LikedProductsByUsers>();
        ProductComments = new List<ProductComment>();
        ShopProfiles = new List<ShopProfile>();
        ShopComments = new List<ShopComment>();
        OrderFeedbacks = new List<OrderFeedback>();
        Carts = new List<Cart>();
        //Orders = new List<Order>();
    }

    public int Id { get; set; }
    [Required]
    [Display(Name = "Ім'я")]
    public string Name { get; set; }
    [Required]
    [Email]
    //[Display(Name = "Е-мейл")]
    public string Email { get; set; }
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }
    [Display(Name = "Роль")]
    public int RoleId { get; set; }
    public int Latitude { get; set; }
    public int Longitude { get; set; }
    public string City { get; set; }
    
    [Display(Name = "Роль")]
    public Role Role { get; set; }
    //public List<Order> Orders { get; set; }
    public List<ShopComment> ShopComments { get; set; }
    public List<ShopProfile> ShopProfiles { get; set; }
    public List<ProductComment> ProductComments { get; set; }
    public List<LikedProductsByUsers> LikedProducts { get; set; }
    public List<OrderFeedback> OrderFeedbacks { get; set; }
    public List<Cart> Carts { get; set; }
}