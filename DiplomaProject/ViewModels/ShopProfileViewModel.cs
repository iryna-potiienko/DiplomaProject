using Microsoft.AspNetCore.Http;

namespace DiplomaProject.ViewModels;

public class ShopProfileViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string City { get; set; }
    public string Address { get; set; }
    public string Username { get; set; }
    
    public IFormFile LogoPhoto { get; set; }
    public string Description { get; set; }
    public string Contacts { get; set; }
}