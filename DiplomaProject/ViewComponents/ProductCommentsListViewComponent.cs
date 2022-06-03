using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.ViewComponents;

public class ProductCommentsListViewComponent: ViewComponent
{
    private readonly KraftWebAppContext _context;

    public ProductCommentsListViewComponent(KraftWebAppContext context)
    {
        _context = context;
    }

    public async Task<ViewViewComponentResult> InvokeAsync(int productId)
    {
        var shopComments = await _context.ProductComments
            .Where(s => s.ProductId == productId)
            .Include(s => s.User)
            .ToListAsync();
        return View("ProductCommentsList", shopComments);
    }
}