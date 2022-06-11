using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.ViewComponents;

public class ShopProfileViewWithProductsViewComponent: ViewComponent
{
    private readonly KraftWebAppContext _context;

    public ShopProfileViewWithProductsViewComponent(KraftWebAppContext context)
    {
        _context = context;
    }

    public async Task<ViewViewComponentResult> InvokeAsync(int? shopProfileId)
    {
        var shopProfile = await _context.ShopProfiles
            .Include(s => s.Salesman)
            .FirstAsync(s => s.Id == shopProfileId);
            
        //ViewBag.ShopProfileId = shopProfile.Id;
        //ViewBag.ShopProfileOwnerEmail = shopProfile.Salesman.Email;
        return View("ShopProfileViewWithProducts",shopProfile);
    }
}