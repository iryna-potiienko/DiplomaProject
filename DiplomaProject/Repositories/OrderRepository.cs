using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.IRepositories;
using DiplomaProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DiplomaProject.Repositories;

public class OrderRepository: IOrderRepository
{
    //private List<CartLine> lineCollection = new List<CartLine>();
    private readonly KraftWebAppContext _context;

    public OrderRepository(KraftWebAppContext context)
    {
        _context = context;
    }

    public async Task<Order> GetOrderById(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        return order;
    }
    public async Task UpdateOrder(Order order)
    {
        _context.Update(order);
        await _context.SaveChangesAsync();
    }
    
    public async void AddProductToOrder(ProductInOrder productInOrder, int quantity)
    {
        //var productInOrder = await _context.ProductsInOrder.FindAsync(game.Id);

        // if (productInOrder == null)
        // {
            _context.Add(productInOrder);
            await _context.SaveChangesAsync();
            // }
        // else
        // {
        //     productInOrder.Quantity += quantity;
        // }
    }

    public async void RemoveProductFromOrder(int id)
    {
        //lineCollection.RemoveAll(l => l.Game.GameId == game.GameId);
        
        var productInOrder = await _context.ProductsInOrder.FindAsync(id);
        _context.ProductsInOrder.Remove(productInOrder);
        await _context.SaveChangesAsync();
    }

    public IIncludableQueryable<Order, OrderFeedback> GetShopOrders(int shopProfileId)
    {
        return _context.Orders
            .Where(o => o.ShopProfileId == shopProfileId)
            .Include(o => o.OrderFeedback);
    }

    // public decimal ComputeTotalValue()
    // {
    //     return lineCollection.Sum(e => e.Game.Price * e.Quantity);
    //
    // }
    //
    // public void Clear()
    // {
    //     lineCollection.Clear();
    // }
    //
    // public IEnumerable<CartLine> Lines
    // {
    //     get { return lineCollection; }
    // }
}

