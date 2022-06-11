using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.ViewComponents;

public class ProductsCardsViewComponent: ViewComponent
{
    private readonly KraftWebAppContext _context;

    public ProductsCardsViewComponent(KraftWebAppContext context)
    {
        _context = context;
    }

    public async Task<ViewViewComponentResult> InvokeAsync(string productAction)
    {
        Task<List<Product>> products;
        switch (productAction)
        {
            case "new":
                products = _context.Products
                    .OrderByDescending(p => p.DateAdded)
                    .ToListAsync();
                break;
            case "popular":
                products = _context.Products
                    .Include(p=>p.ProductInOrders)
                    .OrderByDescending(p => p.ProductInOrders.Count)
                    .ToListAsync();
                break;
            
            default: 
                products = _context.Products.ToListAsync();
                break;
        }

        return View("ProductsCards",await products);
    }

    private void FindPopular()
    {
        var products = _context.Products
            .Include(p=>p.ProductInOrders)
            .OrderByDescending(p => p.ProductInOrders.Count)
            .ToListAsync();
    }
}