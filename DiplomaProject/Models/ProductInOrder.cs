namespace DiplomaProject.Models;

public class ProductInOrder
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public string Amount { get; set; }
    public string Comment { get; set; }
    public double FinalPrice { get; set; }
    public string FinalDescription { get; set; }
    
    public Product Product { get; set; }
    public Order Order { get; set; }
}