using System;
using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class ProductComment
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    
    [Display(Name = "Текст коментаря")]
    public string CommentText { get; set; }
    [Display(Name = "Дата створення")]
    public DateTime Date { get; set; }
    [Display(Name = "Оцінка")]
    public int Estimation { get; set; }
    
    [Display(Name = "Ім'я автора")]
    public User User { get; set; }
    [Display(Name = "Назва товару")]
    public Product Product { get; set; }
}