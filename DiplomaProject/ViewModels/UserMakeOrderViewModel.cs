using System;
using System.Collections.Generic;
using DiplomaProject.Models;

namespace DiplomaProject.ViewModels;

public class UserMakeOrderViewModel
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int DeliveryTypeId { get; set; }
    public string AddressToDelivery { get; set; }
    public string Comment { get; set; }
    public DateTime DateBeReady { get; set; }
    public double Price { get; set; }
    public List<ProductInOrder> ProductsInOrder { get; set; }
}