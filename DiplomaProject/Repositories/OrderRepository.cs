using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Models;

namespace DiplomaProject.Repositories;

public class OrderRepository
{
    //private List<CartLine> lineCollection = new List<CartLine>();
    private readonly KraftWebAppContext _context;

    public OrderRepository(KraftWebAppContext context)
    {
        _context = context;
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

