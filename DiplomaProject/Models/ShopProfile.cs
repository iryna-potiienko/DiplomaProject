using System.Collections.Generic;

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
    public string Name { get; set; }
    public int SalesmanId { get; set; }
    public bool IsVerified { get; set; }
    
    public string City { get; set; }
    public string Address { get; set; }
    public int Latitude { get; set; }
    public int Longitude { get; set; }
    
    public byte[] LogoPhoto { get; set; }
    public string Description { get; set; }
    public string Contacts { get; set; }
    
    public User Salesman { get; set; }
    public List<Product> Products { get; set; }
    public List<ShopComment> ShopComments { get; set; }
    public List<Order> Orders { get; set; }
}