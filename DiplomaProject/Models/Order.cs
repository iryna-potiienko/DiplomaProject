using System;
using System.Collections.Generic;

namespace DiplomaProject.Models;

public class Order
{
    public Order()
    {
        ProductsInOrder = new List<ProductInOrder>();
    }
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ShopProfileId { get; set; }
    public int DeliveryTypeId { get; set; }
    public DateTime DateBeReady { get; set; }
    public DateTime DateOfFixation { get; set; }
    
    public bool IsDelivered { get; set; }
    public string AddressToDelivery { get; set; }
    public string Comment { get; set; }
    public double Price { get; set; }
    public bool IsPaid { get; set; }
    public int ReadyStageId { get; set; }
    public OrderFeedback OrderFeedback { get; set; }
    
    public User Customer { get; set; }
    public ShopProfile ShopProfile { get; set; }
    public DeliveryType DeliveryType { get; set; }
    public ReadyStage ReadyStage { get; set; }
    public List<ProductInOrder> ProductsInOrder { get; set; }
}