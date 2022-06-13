using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DiplomaProject.Controllers;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaProject.ViewModels;

public class UserMakeOrderViewModel
{
    public int Id { get; set; }
    public int CartId { get; set; }
    [Display(Name = "Спосіб доставки")]
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    public int DeliveryTypeId { get; set; }
    [Display(Name = "Адреса доставки")]
    public string AddressToDelivery { get; set; }
    [Display(Name = "Коментар")]
    public string Comment { get; set; }
    [Display(Name = "Дата готовності")]
    [Required(ErrorMessage = "Це поле має бути заповнене")]
    [Remote(action: "VerifyDateBeReady", controller: "Order")]
    public DateTime DateBeReady { get; set; }
    [Display(Name = "Ціна")]
    public double Price { get; set; }
    public List<ProductInOrder> ProductsInOrder { get; set; }
}