using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class DeliveryType
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [Display(Name = "Спосіб доставки")]
    public string Name { get; set; }
}