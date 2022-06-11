using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

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
    [Required]
    [MinLength(3)]
    [Display(Name = "Назва")]
    public string Name { get; set; }
    public DateOnly DateAdded { get; set; }
    public int ShopProfileId { get; set; }
    [Display(Name = "Опис")]
    public string Description { get; set; }
    
    [Display(Name = "Фото")]
    public byte[] Photo { get; set; }
    [Required]
    [MinLength(3)]
    [Display(Name = "Склад продукту")]
    public string Composition { get; set; }
    [Required]
    [Min(0)]
    [Display(Name = "Ціна")]
    public double Price { get; set; }
    [Required]
    [Display(Name = "Підкатегорія")]
    public int SubcategoryId { get; set; }
    [Display(Name = "Підкатегорія")]
    public Subcategory Subcategory { get; set; }
    
    [Display(Name = "Назва магазину")]
    public ShopProfile ShopProfile { get; set; }
    public List<ProductComment> ProductComments { get; set; }
    public List<LikedProductsByUsers> UsersLikes { get; set; }
    public List<ProductInOrder> ProductInOrders { get; set; }
}