using System.Collections.Generic;

namespace DiplomaProject.Models;

public class Product
{
    public Product()
    {
        ProductComments = new List<ProductComment>();
        UsersLikes = new List<LikedProductsByUsers>();
        ProductInOrders = new List<ProductInOrder>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public int ShopProfileId { get; set; }
    public string Description { get; set; }
    
    public string Photo { get; set; }
    public string Composition { get; set; }
    public double Price { get; set; }
    
    public ShopProfile ShopProfile { get; set; }
    public List<ProductComment> ProductComments { get; set; }
    public List<LikedProductsByUsers> UsersLikes { get; set; }
    public List<ProductInOrder> ProductInOrders { get; set; }
}