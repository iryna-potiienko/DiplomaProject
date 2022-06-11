using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class LikedProductsByUsers
{
    public int Id { get; set; }
    [Display(Name = "Товар")]
    public int ProductId { get; set; }
    [Display(Name = "Користувач")]
    public int UserId { get; set; }
    
    [Display(Name = "Користувач")]
    public User User { get; set; }
    [Display(Name = "Товар")]
    public Product Product { get; set; }
}