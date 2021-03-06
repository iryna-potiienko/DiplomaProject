using System;
using System.ComponentModel.DataAnnotations;

namespace DiplomaProject.Models;

public class ShopComment
{
    public int Id { get; set; }
    [Required]
    [MinLength(3)]
    [Display(Name = "Текст коментаря")]
    public string CommentText { get; set; }
    [Display(Name = "Дата надсилання")]
    public DateTime Date { get; set; }
    [Required]
    [Range(1,5)]
    [Display(Name = "Оцінка")]
    public int Estimation { get; set; }
    public int ShopProfileId { get; set; }
    public int UserId { get; set; }
    
    [Display(Name = "Ім'я користувача")]
    public User User { get; set; }
    [Display(Name = "Назва магазину")]
    public ShopProfile ShopProfile { get; set; }
}