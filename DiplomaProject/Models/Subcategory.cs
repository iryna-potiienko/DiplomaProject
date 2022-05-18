using System.Collections.Generic;

namespace DiplomaProject.Models;

public class Subcategory
{
    public Subcategory()
    {
        Products = new List<Product>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public List<Product> Products { get; set; }
}