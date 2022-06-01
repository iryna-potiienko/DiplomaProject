using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    [Display(Name = "Назва")]
    public string Name { get; set; }
    public int ShopProfileId { get; set; }
    [Display(Name = "Опис")]
    public string Description { get; set; }
    
    [Display(Name = "Фото")]
    public byte[] Photo { get; set; }
    [Display(Name = "Склад продукту")]
    public string Composition { get; set; }
    [Display(Name = "Ціна")]
    public double Price { get; set; }
    public int SubcategoryId { get; set; }
    [Display(Name = "Підкатегорія")]
    public Subcategory Subcategory { get; set; }
    
    [Display(Name = "Назва магазину")]
    public ShopProfile ShopProfile { get; set; }
    public List<ProductComment> ProductComments { get; set; }
    public List<LikedProductsByUsers> UsersLikes { get; set; }
    public List<ProductInOrder> ProductInOrders { get; set; }
}