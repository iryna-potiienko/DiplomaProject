using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaProject.Models;

public class Order
{
    public int Id { get; set; }
    [Display(Name = "Назва магазину")]
    public int ShopProfileId { get; set; }
    [Display(Name = "Спосіб доставки")]
    public int DeliveryTypeId { get; set; }
    //[Required(ErrorMessage = "Це поле має бути заповнене")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Замовлення на дату")]
    [Remote(action: "VerifyDateBeReady", controller: "Order")]
    public DateTime DateBeReady { get; set; }
    
    [DataType(DataType.DateTime)]
    [Display(Name = "Дата замовлення")]
    public DateTime DateOfFixation { get; set; }
    
    [Display(Name = "Доставлено")]
    public bool IsDelivered { get; set; }
    
    [Display(Name = "Адреса доставки")]
    public string AddressToDelivery { get; set; }
    [Display(Name = "Коментар покупця")]
    public string UserComment { get; set; }
    [Display(Name = "Коментар продавця")]
    public string SalesmanComment { get; set; }
    [Display(Name = "Ціна")]
    public double Price { get; set; }
    [Display(Name = "Оплачено")]
    public bool IsPaid { get; set; }
    [Display(Name = "Стадія готовності")]
    public int ReadyStageId { get; set; }
    
    [Display(Name = "Відгук покупця")]
    public OrderFeedback OrderFeedback { get; set; }
    
    public int CartId { get; set; }
    public Cart Cart { get; set; }
    [Display(Name = "Назва магазину")]
    public ShopProfile ShopProfile { get; set; }
    [Display(Name = "Спосіб доставки")]
    public DeliveryType DeliveryType { get; set; }
    [Display(Name = "Стадія готовності")]
    public ReadyStage ReadyStage { get; set; }
}