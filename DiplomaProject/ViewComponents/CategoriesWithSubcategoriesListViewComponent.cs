using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.ViewComponents;

public class CategoriesWithSubcategoriesListViewComponent: ViewComponent
{
    private readonly KraftWebAppContext _context;

    public CategoriesWithSubcategoriesListViewComponent(KraftWebAppContext context)
    {
        _context = context;
    }
    
    public async Task<ViewViewComponentResult> InvokeAsync()
    {
        //var subcategories = await _context.Subcategories.Where(s => s.CategoryId == categoryId).ToListAsync();
        //ViewBag.CategoryId = categoryId;

        var categories = _context.Categories
            .Include(c => c.Subcategories)
            .ToListAsync();
        return View("CategoriesWithSubcategoriesListSimple", await categories);
    }
}