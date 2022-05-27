using Microsoft.AspNetCore.Http;

namespace DiplomaProject.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ShopProfileId { get; set; }
    public string Description { get; set; }
    
    public IFormFile Photo { get; set; }
    public string Composition { get; set; }
    public double Price { get; set; }
    public int SubcategoryId { get; set; }
}