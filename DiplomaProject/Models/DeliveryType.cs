using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class DeliveryType
{
    public int Id { get; set; }
    [Display(Name = "Спосіб доставки")]
    public string Name { get; set; }
}