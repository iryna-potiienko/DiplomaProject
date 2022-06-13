using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.ViewComponents;

public class ProductsInOrderListViewComponent: ViewComponent
{
    private readonly KraftWebAppContext _context;

    public ProductsInOrderListViewComponent(KraftWebAppContext context)
    {
        _context = context;
    }
    
    public async Task<ViewViewComponentResult> InvokeAsync(int cartId)
    {
        var productsInOrder = await _context.ProductsInOrder
            .Where(s => s.CartId == cartId)
            .Include(s=>s.Product)
            .ToListAsync();
        //ViewBag.CategoryId = categoryId;

        ViewBag.TotalPrice = productsInOrder.Sum(p => p.Product.Price);
        return View("ProductsInOrderList", productsInOrder);
    }
}