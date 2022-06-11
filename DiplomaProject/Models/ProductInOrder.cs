using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace DiplomaProject.Models;

public class ProductInOrder
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int CartId { get; set; }
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [Min(1,ErrorMessage = "Значення має бути більше 0")]
    [Display(Name = "Кількість")]
    public string Amount { get; set; }
    [Display(Name = "Коментар клієнта")]
    public string Comment { get; set; }
    [Min(0)]
    [Display(Name = "Ціна")]
    public double FinalPrice { get; set; }
    [Display(Name = "Опис від продавця")]
    public string FinalDescription { get; set; }
    
    
    [Display(Name = "Товар")]
    public Product Product { get; set; }
    [Display(Name = "Кошик")]
    public Cart Cart { get; set; }
}