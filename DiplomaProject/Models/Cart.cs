using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class Cart
{
    public Cart()
    {
        ProductsInOrder = new List<ProductInOrder>();
    }
    public int Id { get; set; }
    public int CustomerId { get; set; }
    [Display(Name = "Покупець")]
    public User Customer { get; set; }
    
    public int ShopProfileId { get; set; }
    //public ShopProfile ShopProfile { get; set; }
    public bool IsOpenForAddingProducts { get; set; }
    [Display(Name = "Замовлення у кошику")]
    public List<ProductInOrder> ProductsInOrder { get; set; }
    public Order Order { get; set; }
}