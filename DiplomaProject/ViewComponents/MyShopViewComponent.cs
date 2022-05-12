using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.ViewComponents;

public class MyShopViewComponent: ViewComponent
{
    private readonly KraftWebAppContext _context;

    public MyShopViewComponent(KraftWebAppContext context)
    {
        _context = context;
    }
    
    public async Task<ViewViewComponentResult> InvokeAsync(string currentUserEmail)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
        var shops = await _context.ShopProfiles.Where(s => s.SalesmanId == user.Id).ToListAsync();

        if (shops.Count == 1)
            return View("MyShop", shops.First());
        
        ViewBag.UserId = user.Id;
        return View("MyShops", shops);
    }
}