using System.Collections.Generic;

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
        Orders = new List<Order>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int RoleId{ get; set; }
    public int Latitude { get; set; }
    public int Longitude { get; set; }
    public string Address { get; set; }
    
    public Role Role { get; set; }
    public List<Order> Orders { get; set; }
    public List<ShopComment> ShopComments { get; set; }
    public List<ShopProfile> ShopProfiles { get; set; }
    public List<ProductComment> ProductComments { get; set; }
    public List<LikedProductsByUsers> LikedProducts { get; set; }
    public List<OrderFeedback> OrderFeedbacks { get; set; }
}