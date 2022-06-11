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
    
    [Display(Name = "Магазин")]
    public int ShopProfileId { get; set; }
    
    public bool IsOpenForAddingProducts { get; set; }
    [Display(Name = "Замовлення у кошику")]
    public List<ProductInOrder> ProductsInOrder { get; set; }
    [Display(Name = "Замовлення")]
    public Order Order { get; set; }
}