using System.Collections.Generic;

namespace DiplomaProject.Models;

public class Category
{
    public Category()
    {
        Subcategories = new List<Subcategory>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    
    public List<Subcategory> Subcategories { get; set; }
}