using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class Category
{
    public Category()
    {
        Subcategories = new List<Subcategory>();
    }
    public int Id { get; set; }
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [Display(Name = "Категорія")]
    public string Name { get; set; }
    
    public List<Subcategory> Subcategories { get; set; }
}