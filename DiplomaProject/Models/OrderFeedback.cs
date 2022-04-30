namespace DiplomaProject.Models;

public class OrderFeedback
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int Estimation { get; set; }
    public bool IsInTime { get; set; }
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
    
    public User Customer { get; set; }
    public Order Order { get; set; }
}