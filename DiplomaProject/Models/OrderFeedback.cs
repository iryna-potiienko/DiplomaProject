using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class OrderFeedback
{
    public int Id { get; set; }
    [Display(Name = "Текст коментаря")]
    public string Text { get; set; }
    [Display(Name = "Оцінка")]
    public int Estimation { get; set; }
    [Display(Name = "Чи вчасно?")]
    public bool IsInTime { get; set; }
    [Display(Name = "Чи задоволений клієнт замовленням?")]
    public bool IsEverythingOkay { get; set; }
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
    
    [Display(Name = "Покупець")]
    public User Customer { get; set; }
    [Display(Name = "Замовлення")]
    public Order Order { get; set; }
}