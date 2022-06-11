using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class Subcategory
{
    public Subcategory()
    {
        Products = new List<Product>();
    }
    public int Id { get; set; }
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [Display(Name = "Підкатегорія")]
    public string Name { get; set; }
    public int CategoryId { get; set; }
    [Display(Name = "Категорія")]
    public Category Category { get; set; }
    
    public List<Product> Products { get; set; }
}