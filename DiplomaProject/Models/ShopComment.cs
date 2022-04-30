using System;

namespace DiplomaProject.Models;

public class ShopComment
{
    public int Id { get; set; }
    public string CommentText { get; set; }
    public DateTime Date { get; set; }
    public int Estimation { get; set; }
    public int ShopProfileId { get; set; }
    public int UserId { get; set; }
    
    public User User { get; set; }
    public ShopProfile ShopProfile { get; set; }
}