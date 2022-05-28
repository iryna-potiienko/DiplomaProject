using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.ViewComponents;

public class ShopCommentsListViewComponent: ViewComponent
{
    private readonly KraftWebAppContext _context;

    public ShopCommentsListViewComponent(KraftWebAppContext context)
    {
        _context = context;
    }

    public async Task<ViewViewComponentResult> InvokeAsync(int shopProfileId)
    {
        var shopComments = await _context.ShopComments
            .Where(s => s.ShopProfileId == shopProfileId)
            .Include(s => s.User)
            .ToListAsync();
        return View("ShopCommentsList", shopComments);
    }
}