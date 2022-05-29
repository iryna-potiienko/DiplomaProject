using System.Collections.Generic;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace DiplomaProject.IRepositories;

public interface IOrderRepository
{
    public Task<Order> GetOrderById(int id);

    public Task UpdateOrder(Order order);
    IIncludableQueryable<Order, OrderFeedback> GetShopOrders(int shopProfileId);
}