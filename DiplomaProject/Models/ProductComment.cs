using System;

namespace DiplomaProject.Models;

public class ProductComment
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    
    public string CommentText { get; set; }
    public DateTime Date { get; set; }
    public int Estimation { get; set; }
    
    public User User { get; set; }
    public Product Product { get; set; }
}