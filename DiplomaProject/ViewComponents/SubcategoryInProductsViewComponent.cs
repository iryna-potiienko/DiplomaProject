using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.ViewComponents;

public class SubcategoryInProductsViewComponent: ViewComponent
{
    private readonly KraftWebAppContext _context;

    public SubcategoryInProductsViewComponent(KraftWebAppContext context)
    {
        _context = context;
    }
    
    public async Task<ViewViewComponentResult> InvokeAsync(int categoryId)
    {
        var subcategories = await _context.Subcategories.Where(s => s.CategoryId == categoryId).ToListAsync();
        ViewBag.CategoryId = categoryId;
        return View("SubcategoryInProducts", subcategories);
    }
}